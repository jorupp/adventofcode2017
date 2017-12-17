<Query Kind="Program" />

void Main()
{
	var step = 394;
	var total = 2017;
	var current = 0;
	var list = new List<int>() { 0 };
	for(var i=1; i<= total; i++) {
		current = (current + step) % list.Count;
		list.Insert(current + 1, i);
		current++;
	}
	list[(current + 1) % list.Count].Dump();		
}