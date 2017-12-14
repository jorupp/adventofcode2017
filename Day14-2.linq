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

bool[,] grid = new bool[128, 128];
int[,] regions = new int[128, 128];

bool FillRegion(int num) {
	for(var i=0; i< 128; i++) {
		for(var j=0; j< 128; j++) {
			if(grid[i,j] && 0 == regions[i,j]) {
				FillRegion(num, i, j);
				return true;
			}
		}
	}
	return false;
}

void FillRegion(int region, int i, int j) {
	var stack = new Stack<Tuple<int, int>>();
	var t = new Tuple<int, int>(i, j);
	while(t != null) {
		if(0 <= t.Item1 && t.Item1 < 128) {
			if(0 <= t.Item2 && t.Item2 < 128) {
				if(grid[t.Item1, t.Item2]) {
					if(0 == regions[t.Item1,t.Item2]) {
						regions[t.Item1,t.Item2] = region;
						stack.Push(new Tuple<int, int>(t.Item1 + 1, t.Item2));
						stack.Push(new Tuple<int, int>(t.Item1 - 1, t.Item2));
						stack.Push(new Tuple<int, int>(t.Item1, t.Item2 + 1));
						stack.Push(new Tuple<int, int>(t.Item1, t.Item2 - 1));
					}
				}
			}
		}
		
		if(stack.Count == 0)
			return;
		t = stack.Pop();
	}
}

void Main()
{
	var input = @"nbysizxe";
	
	Enumerable.Range(0, 128).ToList().ForEach(ix => { 
		var e = SolveA(input + "-" + ix); 
		// hmm... bits got a bit scrambled here....
		Enumerable.Range(0, e.Count).ToList().ForEach(i => grid[ix, i] = e.Get((i / 8) * 8 + (7 - i%8) )); 
	});
	
	// sum
	Enumerable.Range(0, 128).SelectMany (x => Enumerable.Range(0, 128).Select (y => new { x, y })).Sum(i => grid[i.x,i.y] ? 1 : 0).Dump();
	
	var region = 1;
	while(true) {
		if(!FillRegion(region))
			break;
		region++;
	}
	(region-1).Dump();
}

// Define other methods and classes here
