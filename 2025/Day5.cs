namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 5)]
public class Day5 : IDayHandler
{
    public void Run(string[] content, out object partOneS, out object partTwoS)
    {
        var rangeCount = content.IndexOf("");
        var ranges = new (long, long)[rangeCount];
        ushort partOne = 0;
        long partTwo = 0;
        long p2c = -1;

        for (int i = 0; i < content.Length; i++)
        {
            if (i < rangeCount)
            {
                var spl = content[i].Split('-');
                ranges[i % rangeCount] = (long.Parse(spl[0]), long.Parse(spl[1]));
            }
        }
        ranges = ranges.OrderBy(e => e.Item1).ToArray();

        for (int i = rangeCount + 1; i < content.Length; i++)
        {
            var v = long.Parse(content[i]);
            for (int x = 0; x < rangeCount; x++)
            {
                if (v >= ranges[x].Item1 && v <= ranges[x].Item2)
                {
                    partOne++;
                    break;
                }
            }
        }

        // part 2
        for (int i = 0; i < ranges.Length; i++)
        {
            if (p2c >= ranges[i].Item1)
                ranges[i].Item1 = p2c + 1;
            if (ranges[i].Item1 <= ranges[i].Item2)
                partTwo += ranges[i].Item2 - ranges[i].Item1 + 1;
            if (ranges[i].Item2 > p2c)
                p2c = ranges[i].Item2;
        }

        partOneS = partOne;
        partTwoS = partTwo;
    }
}