using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(7)]
public class DaySeven : IDayHandler
{
	public void Run(string[] lines)
	{
		var data = ParseInput(lines);
		var a = Test(data, PartOneCheck);
		Console.WriteLine($"Part One: {a}");
		var b = Test(data, PartTwoCheck);
		Console.WriteLine($"Part Two: {b}");
	}
	
	private bool PartOneCheck(long expected, long current, long[] items)
	{
		if (items.Length == 1)
		{
			if ((current + items[0]) == expected)
			{
				return true;
			}

			if ((current * items[0]) == expected)
			{
				return true;
			}
			return false;
		}
		else
		{
			if (PartOneCheck(expected, current + items[0], items.Where((_, i) => i > 0).ToArray()))
			{
				return true;
			}
			if (PartOneCheck(expected, current * items[0], items.Where((_, i) => i > 0).ToArray()))
			{
				return true;
			}

			return false;
		}
	}
	
	private bool PartTwoCheck(long expected, long current, long[] items)
	{
		if (items.Length == 1)
		{
			if ((current + items[0]) == expected)
			{
				return true;
			}

			if ((current * items[0]) == expected)
			{
				return true;
			}

			if (long.Parse($"{current}{items[0]}") == expected)
			{
				return true;
			}

			return false;
		}
		else
		{
			if (PartTwoCheck(expected, current + items[0], items.Where((_, i) => i > 0).ToArray()))
			{
				return true;
			}
			if (PartTwoCheck(expected, current * items[0], items.Where((_, i) => i > 0).ToArray()))
			{
				return true;
			}

			if (PartTwoCheck(expected, long.Parse($"{current}{items[0]}"), items.Where((_, i) => i > 0).ToArray()))
			{
				return true;
			}

			return false;
		}
	}
	
	private long Test(List<(long, List<long>)> originalData, CheckDelegate checkFunc)
	{
		var expectedMatchList = new List<long>();
		
		foreach (var (expected, innerItems) in originalData)
		{
			if (checkFunc(expected, 0, innerItems.ToArray()))
			{
				expectedMatchList.Add(expected);
			}
		}

		var expectedSum = expectedMatchList.Sum();
		return expectedSum;
	}

	public delegate bool CheckDelegate(long expected, long current, long[] items);

	private List<(long, List<long>)> ParseInput(string[] lines)
	{
		var result =  new List<(long, List<long>)>();
		foreach (var item in lines)
		{
			var idx = item.IndexOf(':');
			var keyStr = item.Substring(0, idx);
			var key = long.Parse(keyStr);
			var values = item.Substring(idx + 1).Split(' ').Where(e => !string.IsNullOrEmpty(e)).Select(long.Parse);
			result.Add((key, values.ToList()));
		}

		return result;
	}
}