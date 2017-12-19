<Query Kind="Program" />

void Main()
{
	
	var input = @"     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ ";

	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.ToArray();
	
	Func<int[], char> at = pos => pos[0] < 0 || pos[1] < 0 || pos[1] >= lines.Length || pos[0] >= lines[pos[1]].Length ? ' ' : lines[pos[1]][pos[0]];

	var now = new [] { lines[0].IndexOf('|') , 0 };
	var dir = new [] { 0, 1 };
	var seen = new List<char>();
	var cont = new [] { '|', '+', '-' };
	var turn = new [] { ' ' };
	while(true) {
		var last = now;
		now = add(now, dir);
		var next = add(now, dir);
		var nowC = at(now);
		if(turn.Contains(nowC))
			break;
		var nextC = at(next);
		if(cont.Contains(nowC)) {
			if(nextC == ' ' || nextC == '\0') {
				// turn?
				var options = getAlt(dir).Where(d => { var a = at(add(now, d)); return a != ' ' && a != '\0'; }).ToArray();
				if(options.Length > 1) {
					now.Dump();
					nowC.Dump();
					nextC.Dump();
					dir.Dump();
					options.Select(d => at(add(now, d))).Dump();
				}
				dir = options.SingleOrDefault();
				if(dir == null)
					break;
			}
		} else {
			seen.Add(nowC);
		}
	}
	new string(seen.ToArray()).Dump();
}
int[] add(int[] pos, int[] dir) {
	return new [] { pos[0] + dir[0], pos[1] + dir[1] };
}

int[][] getAlt(int[] dir) {
	if(dir[0] == 0) {
		return new [] { new [] { 1, 0 }, new [] { -1, 0 } };
	} else {
		return new [] { new [] { 0, 1 }, new [] { 0, -1 } };
	}
}

// Define other methods and classes here