using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(16)]
public class DaySixteen : IDayHandler
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
        long result = 0;
        return result;
    }
    public long PartTwo(string[] inputData)
    {
        long result = 0;
        return result;
    }
}