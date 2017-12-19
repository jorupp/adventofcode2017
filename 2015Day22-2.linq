<Query Kind="Program" />

abstract class Node<TNode> where TNode:Node<TNode> {
	public abstract IEnumerable<TNode> GetAdjacent();
	public abstract bool IsValid { get; }
	public abstract bool IsComplete { get; }
	public abstract int CurrentCost { get; }
	public abstract int EstimatedCost { get; }
	public abstract object[] Keys { get; }
	public string Key { get { return string.Join("_", Keys); } }
	public abstract string Description { get; }
}

class GameNode : Node<GameNode> {
	public int PlayerMana;
	public int PlayerHP;
	public int BossHP;
	public int BossDmg;
	public int MoveCount;
	public int ShieldTurnsLeft;
	public int PoisonTurnsLeft;
	public int RechargeTurnsLeft;
	public int SpentMana;
	public bool ShieldActive;
	public bool PoisonActive;
	public bool RechargeActive;
	public bool IsPlayerTurn;
	public Tuple<GameNode, string> Parent;
	public GameNode() {}
	public GameNode(GameNode parent, string change) {
		PlayerMana = parent.PlayerMana;
		PlayerHP = parent.PlayerHP;
		BossHP = parent.BossHP;
		BossDmg = parent.BossDmg;
		MoveCount = parent.MoveCount + 1;
		SpentMana = parent.SpentMana;
		IsPlayerTurn = !parent.IsPlayerTurn;
		if(IsPlayerTurn) {
			this.PlayerHP--;
		}
		Parent = new Tuple<GameNode, string>(parent, change);
		if(parent.ShieldTurnsLeft >= 1) {
			this.ShieldTurnsLeft = parent.ShieldTurnsLeft - 1;
			this.ShieldActive = true;
		}
		if(parent.PoisonTurnsLeft >= 1) {
			this.PoisonTurnsLeft = parent.PoisonTurnsLeft - 1;
			this.BossHP -= 3;
		}
		if(parent.RechargeTurnsLeft >= 1) {
			this.RechargeTurnsLeft = parent.RechargeTurnsLeft - 1;
			this.PlayerMana += 101;
		}
	}
	
	public string LastAction { get { return Parent == null ? null : Parent.Item2; } }
	public override bool IsValid { get { return PlayerHP > 0 && PlayerMana >= 0; } }
	public override bool IsComplete { get { return BossHP <= 0; } }
	public override int CurrentCost { get { return SpentMana + (Parent != null && Parent.Item2 == "Do nothing" ? 1 : 0); } }
	public override int EstimatedCost { get { return CurrentCost + Math.Max(0, (BossHP * 9)); } } // most efficient damage to boss is 9 mana per HP
	public override object[] Keys { get { return new object[] { PlayerMana, PlayerHP, BossHP, BossDmg, CurrentCost, ShieldTurnsLeft, PoisonTurnsLeft, RechargeTurnsLeft, ShieldActive, PoisonActive, RechargeActive, IsPlayerTurn }; } }
	public override string Description { get {
		var states = SelectDeep(this, s => s == null || s.Parent == null || s.Parent.Item1 == null ? new GameNode[0] : new [] { s.Parent.Item1 }).ToList();
		var moves = states.Where(i => i.Parent != null && i.Parent.Item2 != null).Where(i => i.IsPlayerTurn).Select(i => i.Parent.Item2).Reverse().ToList();
		return string.Join(", ", moves);
	} }
	
	public override IEnumerable<GameNode> GetAdjacent() {
		if(this.IsPlayerTurn) {
			// now time for the boss to move
			var bossDead = new GameNode(this, "Boss dies before attacking");
			if(bossDead.IsComplete) {
				yield return bossDead;
			}
			// boss didn't auto-die, let him attack
			var next = new GameNode(this, "Boss attacks");
			next.PlayerHP -= Math.Max(1, next.BossDmg - (next.ShieldActive ? 7 : 0));
			yield return next;
		} else {
			// ok, player move - we have 5 choices...
			{
				var next = new GameNode(this, "Boss dies before player attacks");
				if(next.IsValid && next.IsComplete) {
					// in case the boss died from poison before we acted
					yield return next;
				}
			}
			{
				var next = new GameNode(this, "Magic missle");
				next.PlayerMana -= 53;
				next.SpentMana += 53;
				next.BossHP -= 4;
				yield return next;
			}
			{
				var next = new GameNode(this, "Drain");
				next.PlayerMana -= 73;
				next.SpentMana += 73;
				next.BossHP -= 2;
				next.PlayerHP += 2;
				yield return next;
			}
			if(this.ShieldTurnsLeft <= 1)
			{
				var next = new GameNode(this, "Shield");
				next.PlayerMana -= 113;
				next.SpentMana += 113;
				next.ShieldTurnsLeft = 6;
				yield return next;
			}
			if(this.PoisonTurnsLeft <= 1)
			{
				var next = new GameNode(this, "Poison");
				next.PlayerMana -= 173;
				next.SpentMana += 173;
				next.PoisonTurnsLeft = 6;
				yield return next;
			}
			if(this.RechargeTurnsLeft <= 1)
			{
				var next = new GameNode(this, "Recharge");
				next.PlayerMana -= 229;
				next.SpentMana += 229;
				next.RechargeTurnsLeft = 5;
				yield return next;
			}
		}
	}
}

TNode Evaluate<TNode>(TNode start) where TNode:Node<TNode> {
	var evaluated = new Dictionary<string, TNode>();
	var toEvaluate = new Dictionary<string, TNode>();
	toEvaluate[start.Key] = start;
	
	while(true) {
		var minCompleteCost = evaluated.Where(i => i.Value.IsComplete).Min(i => (int?)i.Value.CurrentCost);
		if(minCompleteCost.HasValue) {
			if(!toEvaluate.Any(i => i.Value.EstimatedCost < minCompleteCost.Value)) {
				// our smallest complete current cost is less than the cost of everything we haven't checked yet, so we're guaranteed optimal
				break;
			}
		}
		
		if(toEvaluate.Count == 0) {
			// error - should never happen
			evaluated.Values.Dump(1);
			return evaluated.Values.OrderByDescending(i => i.CurrentCost).First();
		}
		
		var work = toEvaluate.OrderBy(i => i.Value.EstimatedCost).First().Value;
		toEvaluate.Remove(work.Key);
		evaluated.Add(work.Key, work);
		if(!work.IsComplete) {
			foreach(var next in work.GetAdjacent()) {
				if(!next.IsValid) {
					continue;
				}
				if(evaluated.ContainsKey(next.Key)) {
					if(evaluated[next.Key].CurrentCost > next.CurrentCost || evaluated[next.Key].EstimatedCost > next.EstimatedCost) {
						throw new ArgumentException();
					}
					continue;
				}
				if(toEvaluate.ContainsKey(next.Key)) {
					if(toEvaluate[next.Key].CurrentCost > next.CurrentCost) {
						toEvaluate[next.Key] = next;
					}
					continue;
				}
				toEvaluate.Add(next.Key, next);
			}
		}
	}
//	
//	//evaluated.Values.OrderBy(i => i.CurrentCost).Dump(1);
//	//toEvaluate.Values.Dump();
//	
//	//evaluated.Where(i => i.Value.IsComplete).Select (i => i.Value).OrderBy(i => i.CurrentCost).Dump(1);
//	var parts = new [] { 173, 229, 73, 173, 229, 113, 173, 73, 53 };
//	var sums = Enumerable.Range(0, parts.Length).Select(ix => parts.Take(ix + 1).Sum()).ToArray();
//	sums.Dump();
//
//	var candidates = evaluated.Where(i => sums.Contains(i.Value.CurrentCost)).OrderBy(i => i.Value.CurrentCost).ThenBy(i => i.Value.EstimatedCost).ToArray();
//	//candidates.Select(i => i.Value).Dump(1);
//	
//	var v = evaluated.FirstOrDefault (i => i.Value.CurrentCost == 475 && i.Value.EstimatedCost == 817).Value;
//	new [] { v }.Concat(v.GetAdjacent()).Dump(1);
//	
//	return candidates.First().Value;

	return evaluated.Where(i => i.Value.IsComplete).OrderBy(i => i.Value.CurrentCost).First().Value;
}

public static IEnumerable<T> SelectDeep<T>(T thing, Func<T, IEnumerable<T>> selector) 
{
	yield return thing;
	foreach(var t in selector(thing)) 
	{
		foreach(var d in SelectDeep(t, selector)) {
			yield return d;
		}
	}
}

void Main()
{
//	var initialState = new GameNode() { PlayerHP = 10, PlayerMana = 250, BossHP = 13, BossDmg = 8, IsPlayerTurn = false }; // player goes first
	//var initialState = new GameNode() { PlayerHP = 50, PlayerMana = 500, BossHP = 55, BossDmg = 8, IsPlayerTurn = false }; // player goes first -> sean gets 1289 (I get 1295)
	// ??
//missile	53	1
//drain	73	2
//shield	113	1
//Poison	173	3
//recharge	229	2
	// poison, recharge, drain, poison, recharge, shield, poison, drain, magic missile

	//var initialState = new GameNode() { PlayerHP = 50, PlayerMana = 500, BossHP = 58, BossDmg = 9, IsPlayerTurn = false }; // player goes first -> sean gets 1309
	var finalState = Evaluate(initialState);
	finalState.Dump(1);
	var states = SelectDeep(finalState, s => s == null || s.Parent == null || s.Parent.Item1 == null ? new GameNode[0] : new [] { s.Parent.Item1 }).ToList();
	var moves = states.Where(i => i.Parent != null && i.Parent.Item2 != null).Select(i => string.Format("{0} - {1} -> {2} - {3} {4} {5} - {6} {7}", i.Parent.Item2, i.PlayerMana, i.SpentMana, i.ShieldTurnsLeft, i.PoisonTurnsLeft, i.RechargeTurnsLeft, i.PlayerHP, i.BossHP)).Reverse().ToList();
	moves.Dump();
}

// Define other methods and classes here