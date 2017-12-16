<Query Kind="Program" />

class Layer {
	public int depth;
	public int height;
	public int direction = 1;
	public int position;
}

void Main()
{
	var input = @"x10/0,s2,x6/11";
	var moves = input.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
	var line = "abcdefghijklmnop".ToCharArray();
	var cases = new Dictionary<string, int>();
	for(var i=0; i < 1000000000; i++) {
		foreach(var move in moves) {
			switch(move[0]) {
				case 's':
					var n = int.Parse(move.Substring(1));
					line = line.Skip(line.Length-n).Concat(line.Take(line.Length-n)).ToArray();
					break;
				case 'x':
				{
					var parts = move.Substring(1).Split('/');
					var a = int.Parse(parts[0]);
					var b = int.Parse(parts[1]);
					var t = line[a];
					line[a] = line[b];
					line[b] = t;
				}
					break;
				case 'p':
				{
					var parts = move.Substring(1).Split('/');
					var a = Array.IndexOf(line, parts[0][0]);
					var b = Array.IndexOf(line, parts[1][0]);
					var t = line[a];
					line[a] = line[b];
					line[b] = t;
				}
					break;
			}
		}
		var order = new string(line);
		if(cases.ContainsKey(order)) {
			break;
		}
		cases[order] = i+1;
	}

	cases.Single(i => i.Value == 1000000000 % cases.Count).Key.Dump();
}

// Define other methods and classes here