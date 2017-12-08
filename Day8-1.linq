<Query Kind="Program" />

public class Item {
	public string Target;
	public string Operation;
	public int Delta;
	public string ConditionSource;
	public string Condition;
	public int ConditionValue;
}
Dictionary<string, int> memory = new Dictionary<string, int>();
void Evaluate(Item item) {
	int value = 0;
	memory.TryGetValue(item.ConditionSource, out value);
	if(isMatch(item, value)) {
		value = 0;
		memory.TryGetValue(item.Target, out value);
		switch(item.Operation) {
			case "inc":
				value += item.Delta;
				break;
			case "dec":
				value -= item.Delta;
				break;
			default:
				item.Dump();
				throw new ApplicationException();
		}
		memory[item.Target] = value;
	}
}
bool isMatch(Item item, int value) {
	switch(item.Condition) {
		case ">":
			return value > item.ConditionValue;
		case ">=":
			return value >= item.ConditionValue;
		case "<":
			return value < item.ConditionValue;
		case "<=":
			return value <= item.ConditionValue;
		case "==":
			return value == item.ConditionValue;
		case "!=":
			return value != item.ConditionValue;
	}
	item.Dump();
	throw new ApplicationException();
}

void Main()
{
	var input = @"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10";
	var lines = input.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select (i => {
			var parts = i.Split(' ');
			return new Item {
				Target = parts[0],
				Operation = parts[1],
				Delta = int.Parse(parts[2]),
				ConditionSource = parts[4],
				Condition = parts[5],
				ConditionValue = int.Parse(parts[6]),
			};
		})
		.ToArray();
	foreach(var i in lines) {
		Evaluate(i);
	}
	memory.Max(i => i.Value).Dump();
}

// Define other methods and classes here