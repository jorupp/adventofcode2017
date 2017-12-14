<Query Kind="Program" />

BitArray solve(int[] list, int[] lengths)
{
	int current = 0;
	int skip = 0;
	foreach(var round in Enumerable.Range(0, 64)) {
		foreach(var i in lengths) {
			var start = current;
			var end = (current + i) % list.Length;
			if(start <= end) {
				list = list.Take(current)
					.Concat(list.Skip(current).Take(i).Reverse())
					.Concat(list.Skip(current + i))
					.ToArray();
			}
			else {
				var toReverse = list.Concat(list).Skip(current).Take(i).Reverse().ToArray();
				list = toReverse.Skip(toReverse.Length - end)
					.Concat(list.Skip(end).Take(start-end))
					.Concat(toReverse.Take(toReverse.Length - end))
					.ToArray();
			}
			current += i + skip;
			current %= list.Length;
			skip++;
		}
	}
	var denseHash = Enumerable.Range(0, 16).Select(i => list.Skip(i*16).Take(16).Aggregate(0, (a,ii) => a ^ ii)).ToArray();
	return new BitArray(denseHash.Select (ii => (byte)ii).ToArray());
}

BitArray SolveA(string input) {
	return solve(Enumerable.Range(0,256).ToArray(), input.AsEnumerable().Select (i => (int)i).Concat(new [] {17, 31, 73, 47, 23}).ToArray());
}

void Main()
{
	var input = @"nbysizxe";
	Enumerable.Range(0, 128).Select (e => SolveA(input + "-" + e)).Sum(e => Enumerable.Range(0, e.Count).Sum(i => e.Get(i) ? 1 : 0)).Dump();
	return;
	
}

// Define other methods and classes here
