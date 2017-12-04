<Query Kind="Program" />

void Main()
{
//	GetCoordinates(10).Dump();
//	GetCoordinates(11).Dump();
//	GetCoordinates(12).Dump();
//	GetCoordinates(13).Dump();
//	GetCoordinates(14).Dump();
//	GetCoordinates(15).Dump();
//	GetCoordinates(16).Dump();
//	GetCoordinates(17).Dump();
//	GetCoordinates(24).Dump();
//	GetCoordinates(25).Dump();
//	GetCoordinates(26).Dump();
//	GetCoordinates(27).Dump();

	var data = new List<int>();
	var coords = new List<int[]>();
	data.Add(1);
	coords.Add(new [] { 0, 0 });
	for(var i=2; ; i++)
	{
		var c = GetCoordinates(i);
		var adj = coords.Select((x, ix) => new { x, ix}).Where(x => IsAdjacent(c, x.x)).Select(x => x.ix).ToList();
		//adj.Dump();
		//coords.Dump();
		var value = adj.Sum(x => data[x]);
		
		Console.WriteLine(value);
		if(value > 312051)
		{
			break;
		}

		coords.Add(c);
		data.Add(value);
	}
}
bool IsAdjacent(int[] c1, int[] c2)
{
	return Math.Abs(c1[0] - c2[0]) <=1 && Math.Abs(c1[1] - c2[1]) <= 1;
}

int[] GetCoordinates(int value)
{
	if(value == 1)
		return new [] { 0, 0 };
	var ringPower = Enumerable.Range(0, int.MaxValue).Select(i => i*2+1).Where(i => i*i >= value).FirstOrDefault();
	var sidePower = ringPower - 1;
	var innerRingPower = ringPower -2;
	var sidePosition = (value - innerRingPower*innerRingPower) % sidePower;
	var sideIx = (value - innerRingPower*innerRingPower) / sidePower;
	var ringMoves = sidePower/2;
	var sideMoves = sidePosition - sidePower/2;
	
	switch(sideIx)
	{
		case 4: // bottom right corner
		case 0: return new [] { sidePower/2, sideMoves };
		case 1: return new [] { -sideMoves, sidePower/2 };
		case 2: return new [] { -sidePower/2, -sideMoves };
		case 3: return new [] { sideMoves, -sidePower/2 };
	}
	return new int[0];
}

// bleh... didn't work...  too hard to build this pattern....
int[] GetAdjacentIndexes(int value) 
{
	value.Dump();
	if(value == 1)
		return new int[0];
	if(value == 2)
		return new [] { 1 };
	var ringPower = Enumerable.Range(0, int.MaxValue).Select(i => i*2+1).Where(i => i*i >= value).FirstOrDefault();
	var sidePower = ringPower - 1;
	var innerRingPower = ringPower -2;
	var sidePosition = (value - innerRingPower*innerRingPower) % sidePower;
	var sideIx = (value - innerRingPower*innerRingPower) / sidePower;
	if(sidePosition == 0) // corner
	{
		if(sideIx == 4) // last corner
		{
			return new [] { value - 1, innerRingPower*innerRingPower, innerRingPower*innerRingPower + 1 };
		}
		return new [] { value - 1, (innerRingPower-2)*(innerRingPower-2) + (innerRingPower-1)*sideIx };
	}
	else
	{
		if(sideIx ==0 && sidePosition == 1) 
		{
			// first position of new ring
			return new [] { value - 1, (innerRingPower-2)*(innerRingPower-2)+1 };
		}
		if(sideIx ==0 && sidePosition == 2) 
		{
			// second position of new ring
			return new [] { value - 1, (innerRingPower)*(innerRingPower), (innerRingPower-2)*(innerRingPower-2)+1 };
		}
		if(sidePosition == sidePower - 1) 
		{
			// second position of new ring
			return new [] { value - 1, (innerRingPower)*(innerRingPower), (innerRingPower-2)*(innerRingPower-2)+1 };
		}
		//sidePosition.Dump();
		return new [] { value - 1, (innerRingPower-2)*(innerRingPower-2) + (innerRingPower-1)*sideIx + sidePosition-1, (innerRingPower-2)*(innerRingPower-2) + (innerRingPower-1)*sideIx + sidePosition-2 };
	}
}

// Define other methods and classes here
