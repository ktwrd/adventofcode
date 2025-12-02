using System.ComponentModel;
using System.Diagnostics;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(22)]
public class Day22 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var inputValues = inputData.Select(long.Parse).ToArray();
        var a = PartOne(inputValues);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputValues);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(long[] input)
    {
        long result = 0;
        foreach (var row in input)
        {
            var x = row;
            int i = 0;
            for (;i < 2000; i++)
            {
                x = Process(x);
            }
            result += x;
        }

        return result;
    }

    public long PartTwo(long[] input)
    {
        long result = 0;

        var total = new Dictionary<string, long>();
        foreach (var row in input)
        {
            var x = row;
            var last = x % 10;
            var pattern = new List<(long, long)>();
            for (int i = 0; i < 2000; i++)
            {
                x = Process(x);
                var tmp = x % 10;
                pattern.Add((tmp - last, tmp));
                last = tmp;
            }

            var seen = new HashSet<string>();
            for (int i = 0; i < pattern.Count - 4; i++)
            {
                var pat = pattern.GetRange(i, 4).Select(e => e.Item1).ToArray();
                var patString = string.Join(" ", pat);
                var val = pattern[i + 3].Item2;
                if (seen.Contains(patString) == false)
                {
                    seen.Add(patString);
                    if (total.ContainsKey(patString) == false)
                    {
                        total[patString] = val;
                    }
                    else
                    {
                        total[patString] += val;
                    }
                }
            }
        }

        return total.Values.OrderByDescending(e => e).First();
        
        return result;
    }
    public long Process(long inp)
    {
        var x = ((inp * 64) ^ inp) % 16777216;
        x = (Convert.ToInt64(Math.Floor(x / (decimal)32)) ^ x) % 16777216;
        x = ((x * 2048) ^ x) % 16777216;
        return x;
    }
}