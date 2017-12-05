<Query Kind="Statements" />

var input = @"abcde fghij
abcde xyz ecdab
a ab abc abd abf abj
iiii oiii ooii oooi oooo
oiii ioii iioi iiio";
var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select (i => i.Split(new [] { ' ', '\t'} ).Select(ii => new string(ii.AsEnumerable().OrderBy(iii => iii).ToArray())).ToList())
	.ToList();
	
lines.Count(i => i.GroupBy(ii => ii).All(ii => ii.Count() == 1)).Dump();
