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
	
	for(var i=0; i< 5000000; i++) {
		do {
			genA = (genA * genAInc) % div;
		} while(genA % 4 != 0);
		do {
			genB = (genB * genBInc) % div;
		} while(genB % 8 != 0);
		if((genA & judgeMask) == (genB & judgeMask)) {
			judge++;
		}
	}
	
	judge.Dump();
}

// Define other methods and classes here