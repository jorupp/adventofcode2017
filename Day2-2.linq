<Query Kind="Program" />

IEnumerable<int[]> GetPairs(ICollection<int> input) 
{
	return input.SelectMany((i, ix) => input.Skip(ix+1).Select(ii => new [] { Math.Max(i, ii), Math.Min(i, ii) })).ToList();
}
void Main()
{
	var input = @"5 9 2 8
9 4 7 3
3 8 6 5";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => i.Split(new [] { ' ', '\t'} ).Select(ii => int.Parse(ii)).ToList())
		.ToList();
	//lines.Dump();
	lines.Sum(i => GetPairs(i).Where(ii => ii[0] % ii[1] == 0).Select(ii => ii[0] / ii[1]).Single()).Dump();
	
}

// Define other methods and classes here
