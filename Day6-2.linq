<Query Kind="Program" />

int[] memory;
HashSet<ulong> seen = new HashSet<ulong>();
void Main()
{
	//var input = @"0 2 7 0";
	var input = @"2	8	8	5	4	2	3	1	5	5	1	2	15	13	5	14";
	memory = input.Split(new [] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => int.Parse(i))
		.ToArray();
	
	var ii=0;
	for(; !seen.Contains(Status()); ii++)
	{
		seen.Add(Status());
		Redistribute();
	}

	ii=0;
	seen = new HashSet<ulong>();
	for(; !seen.Contains(Status()); ii++)
	{
		seen.Add(Status());
		Redistribute();
	}
	
	ii.Dump();
}

ulong Status()
{
	ulong v = 0;
	for(var i=0; i<memory.Length; i++)
	{
		v += (ulong)memory[i] << i*4;
	}
	return v;
}

void Redistribute() 
{
	var ix = Array.IndexOf(memory, memory.Max());
	var value = memory[ix];
	memory[ix] = 0;
	for(var i=0; i <value; i++)
	{
		ix++;
		ix %= memory.Length;
		memory[ix]++;
	}
}

// Define other methods and classes here
