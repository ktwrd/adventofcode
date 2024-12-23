using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(23)]
public class Day23 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    private long PartOne(string[] inputData)
    {
        return 0;
    }

    private long PartTwo(string[] inputData)
    {
        return 0;
    }
}