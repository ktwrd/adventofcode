namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 12)]
public class Day12 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var partIndex = 0;
        var result = 0;

        var parts = new byte[content.Count(e => e.EndsWith(':'))];
        var lastWhitespace = content.LastIndexOf("");
        foreach (var (row, line) in content.Index())
        {
            if (row == lastWhitespace) continue;
            if (row < lastWhitespace)
            {
                if (line.Length == 0)
                {
                    partIndex++;
                }
                else
                {
                    parts[partIndex] += (byte)line.Count(e => e == '#');
                }
            }
            else
            {
                // sorry for the fat comment block
                // i promise this isn't ai generated lol

                // only required if using example input (uses ~507kb mem for my solution)
                // var xi = line.IndexOf('x');
                // var ci = line.IndexOf(':');
                // var w = int.Parse(line[..xi]);
                // var h = int.Parse(line[(xi + 1)..ci]);

                // your real input should always have 2 characters
                // for width, and 2 characters for height. like "30x58"
                // so its hard coded for slightly better mem usage
                // for me.

                // your real input should always have 2 characters
                // for width, and 2 characters for height. like "30x58"
                // so its hard coded for slightly better mem usage
                // for me. byte is also used instead of int to improve
                // memory usage and performance. ushort was used so
                // there isn't any overflow issues, and the max
                // possible value fits within the ushort limit.
                // (assuming 2 chars per number, and all 6 shapes
                // with 7 for the value)
                //      mean: 174.5us ->  154us
                // allocated: 507kb   ->  477.59kb
                // max value: a=9801, s=4158
                var w = ushort.Parse(line[..2]);
                var h = ushort.Parse(line[3..5]);
                var a = w * h;
                var s = line[7..]
                    .Split(' ')
                    .Select((e, i) => ushort.Parse(e) * parts[i])
                    .Sum();
                if (a >= s)
                {
                    result++;
                }
            }
        }

        partOne = result;
        partTwo = "happy holidays :3";
    }
}