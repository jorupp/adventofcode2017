<Query Kind="Program" />

class Layer {
	public int depth;
	public int height;
	public int direction = 1;
	public int position;
}

void Main()
{
	var input = @"0: 4
	1: 2
	2: 3
	4: 4
	6: 6
	8: 5
	10: 6
	12: 6
	14: 6
	16: 12
	18: 8
	20: 9
	22: 8
	24: 8
	26: 8
	28: 8
	30: 12
	32: 10
	34: 8
	36: 12
	38: 10
	40: 12
	42: 12
	44: 12
	46: 12
	48: 12
	50: 14
	52: 14
	54: 12
	56: 12
	58: 14
	60: 14
	62: 14
	66: 14
	68: 14
	70: 14
	72: 14
	74: 14
	78: 18
	80: 14
	82: 14
	88: 18
	92: 17";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => i.Split(new [] {':', ' '}, StringSplitOptions.RemoveEmptyEntries).Select(ii => int.Parse(ii)).ToArray())
		.Select(i => new Layer() { depth = i[0], height = i[1], position = 0, direction = 1 })
		.ToArray();
		
	var caught = lines.Where(i => i.depth % ((i.height-1)*2) == 0).ToList();
	caught.Sum(i => i.depth * i.height).Dump();
}

// Define other methods and classes here