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
        var result = BlinkItemAltAgain(data, count);

        return result;
    }

    public long BlinkItemAltAgain(long[] inputData, int limit)
    {
        long total = 0;
        for (int ixi = 0; ixi < inputData.Length; ixi++)
        {
            int current = 0;
            var data = new long[] { inputData[ixi] };
            while (current < limit)
            {
                long expectedLength = 0;
                foreach (var v in data)
                {
                    expectedLength++;
                    if (v != 0)
                    {
                        var vs = v.ToString().Length;
                        if (vs % 2 == 0)
                        {
                            expectedLength++;
                        }
                    }
                }

                long index = 0;
                var result = new long[expectedLength];
                foreach (var v in data)
                {
                    if (v == 0)
                    {
                        result[index] = 1;
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

                            result[index] = long.Parse(left);
                            index++;
                            result[index] = string.IsNullOrEmpty(right) ? 0 : long.Parse(right);
                        }
                        else
                        {
                            result[index] = v * 2024;
                        }
                        index++;
                    }
                }

                data = result;
                current++;
                Console.WriteLine($"{ixi}: {current}/{limit} ({data.LongLength})");
            }

            total += data.LongLength;
            data = [];
            Console.WriteLine($"{ixi}/{inputData.Length}");
        }

        return total;
    }
    public long BlinkItemAlt(long[] inputData, int limit)
    {
        int current = 0;
        var data = inputData;
        while (current < limit)
        {
            long expectedLength = 0;
            foreach (var v in data)
            {
                expectedLength++;
                if (v != 0)
                {
                    var vs = v.ToString().Length;
                    if (vs % 2 == 0)
                    {
                        expectedLength++;
                    }
                }
            }

            long index = 0;
            var result = new long[expectedLength];
            foreach (var v in data)
            {
                if (v == 0)
                {
                    result[index] = 1;
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

                        result[index] = long.Parse(left);
                        index++;
                        result[index] = string.IsNullOrEmpty(right) ? 0 : long.Parse(right);
                    }
                    else
                    {
                        result[index] = v * 2024;
                    }
                    index++;
                }
            }

            data = result;
            current++;
        }

        return data.LongLength;
    }
    public long[] BlinkItem(long[] input)
    {
        long expectedLength = 0;
        foreach (var v in input)
        {
            if (v != 0)
            {
                var vs = v.ToString().Length;
                if (vs % 2 == 0)
                {
                    expectedLength++;
                }
            }
            expectedLength++;
        }

        long index = 0;
        var result = new long[expectedLength];
        foreach (var v in input)
        {
            if (v == 0)
            {
                result[index] = 0;
            }
            else
            {
                var vs = v.ToString().Length;
                if (vs % 2 == 0)
                {
                    var vstr = v.ToString();
                    var len = Convert.ToInt32(Math.Floor(vstr.Length / 2f));
                    var left = vstr[..len];

                    result[index] = int.Parse(left);
                    left = string.Empty;
                    index++;

                    var right = vstr[len..].TrimStart('0');
                    result[index] = right.Length > 0 ? int.Parse(right) : 0;
                    right = string.Empty;
                    vstr = string.Empty;
                }
                else
                {
                    result[index] = v * 2024;
                }
            }
            index++;
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
