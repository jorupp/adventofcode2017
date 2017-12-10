<Query Kind="Program" />

int solve(int[] list, int[] lengths)
{
	int current = 0;
	int skip = 0;
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
	return list[0] * list [1];
}

void Main()
{
	solve(Enumerable.Range(0,5).ToArray(), new [] { 3, 4, 1, 5}).Dump();
	solve(Enumerable.Range(0,256).ToArray(), new [] {183,0,31,146,254,240,223,150,2,206,161,1,255,232,199,88}).Dump();
}

// Define other methods and classes here