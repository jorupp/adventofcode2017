<Query Kind="Program" />

void Main()
{
	GetSteps(9).Dump();
	GetSteps(10).Dump();
	GetSteps(1).Dump();
	GetSteps(12).Dump();
	GetSteps(23).Dump();
	GetSteps(1024).Dump();
	GetSteps(312051).Dump();
}

int GetSteps(int value) 
{
	if(value == 1)
		return 0;
	var ringPower = Enumerable.Range(0, int.MaxValue).Select(i => i*2+1).Where(i => i*i >= value).FirstOrDefault();
	var sidePower = ringPower - 1;
	var innerRingPower = ringPower -2;
	var sidePosition = (value - innerRingPower*innerRingPower) % sidePower;
	var ringMoves = sidePower/2;
	var sideMoves = Math.Abs(sidePosition - sidePower/2);
	//new [] { ringPower, sidePower, innerRingPower, sidePosition, ringMoves, sideMoves }.Dump();
	return ringMoves + sideMoves;

	// bottom right corner of ring is ringPower^2
	// directly above it is (ringPower-2)^2+1, with 4 sidePower sides
	
	// center of each side is +sidePower/2, corner is +sidePower
	
	// (r-2)*(r-2)+4(r-1) = r*r
	// r*r-4r+4+r-4 = r*r
}
