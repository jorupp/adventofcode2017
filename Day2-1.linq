<Query Kind="Statements" />

var input = @"5 1 9 5
7 5 3
2 4 6 8";
var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select (i => i.Split(new [] { ' ', '\t'} ).Select(ii => int.Parse(ii)).ToList())
	.ToList();
//lines.Dump();
lines.Sum(i => i.Max() - i.Min()).Dump();