<Query Kind="Program" />

void Main()
{
	long genA = 289;
	long genB = 629;
	
	long genAInc = 16807;
	long genBInc = 48271;
	
	long div = 2147483647;
	
	long judge = 0;
	long judgeMask = 0xffff;
	
	for(var i=0; i< 40000000; i++) {
		genA = (genA * genAInc) % div;
		genB = (genB * genBInc) % div;
		if((genA & judgeMask) == (genB & judgeMask)) {
			judge++;
		}
	}
	
	judge.Dump();
}

// Define other methods and classes here