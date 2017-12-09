<Query Kind="Program" />

int TotalScore(string input) {
	var totalScore = 0;
	var currentGroupScore = 0;
	var garbage = 0;
	for(var i=0; i<input.Length; i++) {
		var current = input[i];
		if(current == '<')
		{
			i++;
			for(;; i++) {
				if(input[i] == '>')
					break;	
				if(input[i] == '!')
					i++;
				else
					garbage++;
			}
		} else if(current == '{') {
			currentGroupScore++;
			totalScore += currentGroupScore;
		} else if(current == '}') {
			currentGroupScore--;
		} else if(current == ',') {
		} else {
			i.Dump();
			current.Dump();
			throw new ArgumentException();
		}
	}
	return garbage;
}

void Main()
{
	TotalScore(@"{}").Dump();
	TotalScore(@"{{{}}}").Dump();
	TotalScore(@"{{},{}}").Dump();
	TotalScore(@"{{{},{},{{}}}}").Dump();
	TotalScore(@"{<a>,<a>,<a>,<a>}").Dump();
	TotalScore(@"{{<ab>},{<ab>},{<ab>},{<ab>}}").Dump();
	TotalScore(@"{{<!!>},{<!!>},{<!!>},{<!!>}}").Dump();
	TotalScore(@"{{<a!>},{<a!>},{<a!>},{<ab>}}").Dump();
}

// Define other methods and classes here
