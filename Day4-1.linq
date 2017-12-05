<Query Kind="Statements" />

var input = @"aa bb cc dd ee
aa bb cc dd aa
aa bb cc dd aaa";
var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select (i => i.Split(new [] { ' ', '\t'} ).ToList())
	.ToList();
	
lines.Count(i => i.GroupBy(ii => ii).All(ii => ii.Count() == 1)).Dump();
