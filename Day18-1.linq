<Query Kind="Program" />

int getReg(string x) {
	if(x.Length != 1)
		throw new ArgumentException();
	return (int)(x[0] - 'a');
}
void Main()
{
	var input = @"set i 31
set a 1
mul p 17
jgz p p
mul a 2
add i -1
jgz i -2
add a -1
set i 127
set p 618
mul p 8505
mod p a
mul p 129749
add p 12345
mod p a
set b p
mod b 10000
snd b
add i -1
jgz i -9
jgz a 3
rcv b
jgz b -1
set f 0
set i 126
rcv a
rcv b
set p a
mul p -1
add p b
jgz p 4
snd a
set a b
jgz 1 3
snd b
set f 1
add i -1
jgz i -11
snd a
jgz f -16
jgz a -19";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => i.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries).ToArray())
		.ToArray();
	
	var registers = Enumerable.Range(0, 26).Select(i => (long)0).ToArray();
	
	Func<string, long> getVal = i => {
		int val;
		if(int.TryParse(i, out val)) {
			return val;
		}
		return registers[getReg(i)];
	};
	
	long lastPlayed = 0;
	for(long i=0; i<lines.Length; i++) {
		var line = lines[i];
		switch(line[0]) {
			case "snd":
				lastPlayed = getVal(line[1]);
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
				if(getVal(line[1]) != 0) {
					lastPlayed.Dump();
					return;
				}
				break;
			case "jgz":
				if(getVal(line[1]) > 0) {
					i += getVal(line[2]) - 1;
				}
				break;
		}
	}
	
}

// Define other methods and classes here