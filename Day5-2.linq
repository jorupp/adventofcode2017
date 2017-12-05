<Query Kind="Statements" />

var input = @"0
3
0
1
-3";
var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select (i => int.Parse(i))
	.ToArray();
	
var steps = 0;
var ix = 0;
while(true)
{
	steps++;
	var oldIx = ix;
	ix += lines[ix];
	if(lines[oldIx] >= 3)
		lines[oldIx]--;
	else
		lines[oldIx]++;
	if(ix < 0 || ix >= lines.Length)
		break;
}
steps.Dump();
