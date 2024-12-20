using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(19)]
public class Day19 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(string[] inputData)
    {
        var (towels, designs) = ParseData(inputData);
        long result = 0;
        foreach (var d in designs)
        {
            if (SolvePartOne(towels, d, []))
            {
                result++;
            }
        }
        return result;
    }

    public long PartTwo(String[] inputData)
    {
        var (towels, designs) = ParseData(inputData);
        long result = 0;
        foreach (var d in designs)
        {
            result += SolvePartTwo(designs, towels, d, []);
        }

        return result;
    }
    
    public (string[] towels, string[] designs) ParseData(string[] inputData)
    {
        var towels = Array.Empty<string>();
        var designs = new List<string>();
        for (int i = 0; i < inputData.Length; i++)
        {
            if (i == 0)
            {
                towels = inputData[i].Split(',').Select(e => e.Trim()).ToArray();
            }
            else if (!string.IsNullOrEmpty(inputData[i]))
            {
                designs.Add(inputData[i]);
            }
        }

        return (towels, designs.ToArray());
    }

    public bool SolvePartOne(string[] towels, string design, Dictionary<string, bool> memo)
    {
        if (string.IsNullOrEmpty(design))
            return true;
        if (memo.ContainsKey(design))
            return memo[design];


        foreach (var p in towels)
        {
            if (design.StartsWith(p))
            {
                if (SolvePartOne(towels, design.Slice(p.Length), memo))
                {
                    memo[design] = true;
                    return true;
                }
            }
        }

        memo[design] = false;
        return false;
    }
    public long SolvePartTwo(string[] designs, string[] towels, string design, Dictionary<string, long> memo)
    {
        if (string.IsNullOrEmpty(design))
            return 1;
        if (memo.ContainsKey(design))
            return memo[design];


        long ways = 0;
        foreach (var p in towels)
        {
            if (design.StartsWith(p))
            {
                ways += SolvePartTwo(designs, towels, design.Slice(p.Length), memo);
            }
        }

        memo[design] = ways;
        return ways;
    }
}