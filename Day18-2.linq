<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

int getReg(string x) {
	if(x.Length != 1)
		throw new ArgumentException();
	return (int)(x[0] - 'a');
}
void Main()
{
	var input = @"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => i.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries).ToArray())
		.ToArray();
		
	long written1 = 0;
	long written2 = 0;
	bool waiting1 = false;
	bool waiting2 = false;
	var col1 = new BlockingCollection<long>();
	var col2 = new BlockingCollection<long>();
	
	new Thread(() => RunIt(lines, col1, col2, 0, ref written1, ref waiting1)).Start();
	new Thread(() => RunIt(lines, col2, col1, 1, ref written2, ref waiting2)).Start();
	
	while(true) {
		Thread.MemoryBarrier();
		if(waiting1 && waiting2) {
			Thread.Sleep(100);
			Thread.MemoryBarrier();
			if(waiting1 && waiting2) {
				"done".Dump();
				Thread.MemoryBarrier();
				written2.Dump();
				return;
			}
		}
	}
}

void RunIt(string[][] lines, BlockingCollection<long> input, BlockingCollection<long> output, int p, ref long valuesWritten, ref bool waiting) {
	var registers = Enumerable.Range(0, 26).Select(i => (long)0).ToArray();
	
	Func<string, long> getVal = i => {
		int val;
		if(int.TryParse(i, out val)) {
			return val;
		}
		return registers[getReg(i)];
	};
	
	registers[getReg("p")] = p;
	
	for(long i=0; i<lines.Length; i++) {
		var line = lines[i];
		switch(line[0]) {
			case "snd":
				output.Add(getVal(line[1]));
				valuesWritten++;
				break;
			case "set":
				registers[getReg(line[1])] = getVal(line[2]);
				break;
			case "add":
				registers[getReg(line[1])] += getVal(line[2]);
				break;
			case "mul":
				registers[getReg(line[1])] *= getVal(line[2]);
				break;
			case "mod":
				registers[getReg(line[1])] %= getVal(line[2]);
				break;
			case "rcv":
				waiting = true;
				Thread.MemoryBarrier();
				registers[getReg(line[1])] = input.Take();
				waiting = false;
				Thread.MemoryBarrier();
				break;
			case "jgz":
				if(getVal(line[1]) > 0) {
					i += getVal(line[2]) - 1;
				}
				break;
		}
	}

}