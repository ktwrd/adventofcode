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
    
    public long PartTwo(string[] inputData)
    {
        var data = ParseInput(inputData);
        var result = Blink(75, data);
        return result;
    }

    public long Blink(int count, long[] data)
    {
        long result = 0;
        foreach (var v in data)
        {
            result += CountChildren(v, count);
        }

        return result;
    }

    public long CountChildren(long value, long remaining)
    {
        if (remaining == 0)
            return 1;
        
        var storeKey = (value, remaining);
        if (Store.TryGetValue(storeKey, out var storeResult))
            return storeResult;

        long result = 0;

        if (value == 0)
        {
            result = CountChildren(1, remaining - 1);
        }
        else
        {
            var valueStr = value.ToString();
            if (valueStr.Length % 2 == 0)
            {
                var len = Math.Max(Convert.ToInt32(Math.Floor(valueStr.Length / 2f)), 0);
                var leftString = valueStr.Substring(0, len);
                var rightString = valueStr.Substring(len).TrimStart('0');
                
                var left = long.Parse(leftString);
                var right = string.IsNullOrEmpty(rightString) ? 0 : long.Parse(rightString);
                result = CountChildren(left, remaining - 1);
                result += CountChildren(right, remaining - 1);
            }
            else
            {
                result = CountChildren(value * 2024, remaining - 1);
            }
        }

        Store[storeKey] = result;
        return result;
    }

    private Dictionary<(long value, long remaining), long> Store = [];
}
