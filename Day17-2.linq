<Query Kind="Program" />

void Main()
{
	var step = 394;
	var total = 50000000;
	var current = 0;
	var listLength = 1;
	// since we only need to know the value after zero, and we know that zero was the first value and we only insert after our pointer, we know 0 will remain the first value.
	// so, we just keep track of the most recent value we would have put in that slot,
	// which means we only need to track that and the current list length - way less data to keep track of and possible to calculate pretty quickly.
	var lastAfterZero = 0;
	for(var i=1; i<= total; i++) {
		current = (current + step) % listLength;
		if(current == 0)
			lastAfterZero = i;
		listLength++;
		current++;
	}
	lastAfterZero.Dump();
}