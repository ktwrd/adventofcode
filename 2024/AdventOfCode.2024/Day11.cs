using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(11)]
public class DayEleven : IDayHandler
{
	public void Run(string[] lines)
	{
		var a = PartOne(lines);
		Console.WriteLine($"Part One: {a}");
		var b = PartTwo(lines);
		Console.WriteLine($"Part Two: {b}");
	}

	public long[] ParseInput(string[] inputData)
	{
		return string.Join(' ', inputData).Split(" ").Select(long.Parse).ToArray();
	}

	public long PartOne(string[] lines)
	{
		var data = ParseInput(lines);
		var result = Blink(25, data);
		return result;
	}

	public long Blink(int count, long[] data)
	{
		long result = 0;
		for (int i = 0; i < count; i++)
		{
			data = BlinkItem(data);
			Console.WriteLine($"{i}: {data.LongLength}");
			result = data.LongLength;
		}

		return result;
	}
	public long[] BlinkItem(long[] input)
	{
		var result = new List<long>();
		foreach (var v in input)
		{
			if (v == 0)
			{
				result.Add(1);
			}
			else
			{
				var vs = v.ToString();
				if (vs.Length % 2 == 0)
				{
					var vstr = v.ToString();
					var len = Math.Max(Convert.ToInt32(Math.Floor(vstr.Length / 2f)), 0);
					var left = vstr.Substring(0, len);
					var right = vstr.Substring(len).TrimStart('0');
				
					if (left.Length > 0)
					{
						result.Add(int.Parse(left));
					}
					if (right.Length > 0)
					{
						result.Add(int.Parse(right));
					}
					else
					{
						result.Add(0);
					}
				}
				else
				{
					result.Add(v * 2024);
				}
			}
		}

		return result.ToArray();
	}
	
	public long PartTwo(string[] inputData)
	{
		var data = ParseInput(inputData);
		var result = Blink(75, data);
		return result;
	}
}
