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
        long expectedLength = 0;
        foreach (var v in input)
        {
            if (v == 0)
            {
                expectedLength++;
            }
            else
            {
                var vs = v.ToString();
                if (vs.Length % 2 == 0)
                {
                    expectedLength += 2;
                }
                else
                {
                    expectedLength++;
                }
            }
        }

        long index = 0;
        long[] result = new long[expectedLength];
        foreach (var v in input)
        {
            if (v == 0)
            {
                result[index] = 0;
                index++;
            }
            else
            {
                var vs = v.ToString().Length;
                if (vs % 2 == 0)
                {
                    var vstr = v.ToString();
                    var len = Math.Max(Convert.ToInt32(Math.Floor(vstr.Length / 2f)), 0);
                    var left = vstr.Substring(0, len);
                    var right = vstr.Substring(len).TrimStart('0');

                    result[index] = int.Parse(left);
                    index++;
                    result[index] = right.Length > 0 ? int.Parse(right) : 0;
                    index++;
                }
                else
                {
                    result[index] = v * 2024;
                    index++;
                }
            }
        }

        return result;
    }
    
    public long PartTwo(string[] inputData)
    {
        var data = ParseInput(inputData);
        var result = Blink(75, data);
        return result;
    }
}
