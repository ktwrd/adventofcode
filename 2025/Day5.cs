namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 5)]
public class Day5 : IDayHandler
{
    public void Run(string[] content)
    {
        var rangeCount = content.IndexOf("");
        var ranges = new long[rangeCount][];
        long partOne = 0;
        long partTwo = 0;
        long p2c = -1;

        for (int i = 0; i < content.Length; i++)
        {
            if (i < rangeCount)
            {
                var spl = content[i].Split('-');
                ranges[i % rangeCount] = [long.Parse(spl[0]), long.Parse(spl[1])];
            }
        }
        ranges = ranges.OrderBy(e => e[0]).ToArray();

        // part 1
        for (int i = rangeCount + 1; i < content.Length; i++)
        {
            var v = long.Parse(content[i]);
            for (int x = 0; x < rangeCount; x++)
            {
                if (v >= ranges[x][0] && v <= ranges[x][1])
                {
                    partOne++;
                    break;
                }
            }
        }

        // part 2
        for (int i = 0; i < ranges.Length; i++)
        {
            if (p2c >= ranges[i][0])
                ranges[i][0] = p2c + 1;
            if (ranges[i][0] <= ranges[i][1])
                partTwo += ranges[i][1] - ranges[i][0] + 1;
            p2c = Math.Max(p2c, ranges[i][1]);
        }

        Console.WriteLine($"Part One: {partOne}");
        Console.WriteLine($"Part Two: {partTwo}");
    }
}