<Query Kind="Program" />

string solve(int[] list, int[] lengths)
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
	return string.Join("", denseHash.Select (ii => string.Format("{0:x2}", ii)).ToArray());
}

string SolveA(string input) {
	return solve(Enumerable.Range(0,256).ToArray(), input.AsEnumerable().Select (i => (int)i).Concat(new [] {17, 31, 73, 47, 23}).ToArray());
}

void Main()
{
	//(65 ^ 27 ^ 9 ^ 1 ^ 4 ^ 3 ^ 40 ^ 50 ^ 91 ^ 7 ^ 6 ^ 0 ^ 2 ^ 5 ^ 68 ^ 22).Dump();
	SolveA("").Dump();
	SolveA("AoC 2017").Dump();
	SolveA("1,2,3").Dump();
	SolveA("1,2,4").Dump();
	SolveA("183,0,31,146,254,240,223,150,2,206,161,1,255,232,199,88").Dump();
}

// Define other methods and classes here
