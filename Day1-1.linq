<Query Kind="Program" />

void Main()
{
	Run("1122").Dump();
	Run("1111").Dump();
	Run("1234").Dump();
	Run("91212129").Dump();
}

int Run(string input) {
	var data = input.Select(i => int.Parse(i.ToString())).ToList();
	var result = data.Zip(data.Skip(1).Concat(new [] {data[0]}), (i1, i2) => new { i1, i2 })
	.Where(i => i.i1 == i.i2).Sum(i => i.i1);
	return result;
}

// Define other methods and classes here
