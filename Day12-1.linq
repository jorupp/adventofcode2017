<Query Kind="Program" />

void Find(int start, Dictionary<int, int[]> lines, HashSet<int> seen) {
	if(seen.Contains(start))
		return;
	seen.Add(start);
	foreach(var i in lines[start]) {
		Find(i, lines, seen);
	}
}

void Main()
{
	var input = @"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => i.Split(new [] { ' ', '<', '-', '>', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ii => int.Parse(ii)).ToArray())
		.ToDictionary(i => i[0], i => i.Skip(1).ToArray());
	var seen = new HashSet<int>();
	Find(0, lines, seen);
	seen.Count.Dump();
}

// Define other methods and classes here