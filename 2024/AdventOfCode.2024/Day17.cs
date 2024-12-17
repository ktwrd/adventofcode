using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(17)]
public class DaySeventeen : IDayHandler
{
	public void Run(string[] inputData)
	{
		var a = PartOne(inputData);
		Console.WriteLine($"Part One: {a}");
		var b = PartTwo(inputData);
		Console.WriteLine($"Part Two: {b}");
	}

	public long PartOne(string[] lines)
	{
		return 0;
	}
	public long PartTwo(string[] lines)
	{
		return 0;
	}
}
