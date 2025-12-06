using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 2)]
public class Day2 : IDayHandler
{
    public void Run(string[] contentL, out object partOneS, out object partTwoS)
    {
        var content = string.Join("", contentL)
            .Split(',')
            .Select(e =>
            {
                var s = e.Split('-');
                return new[]
                {
                    long.Parse(s[0]),
                    long.Parse(s[1])
                };
            })
            .ToArray();
        long partOne = 0;
        long partTwo = 0;
        for (int i = 0; i < content.Length; i++)
        {
            for (long j = content[i][0]; j <= content[i][1]; j++)
            {
                var js = j.ToString();
                var half = js.Length / 2;
                if (js[half..] == js[..half])
                {
                    partOne += j;
                    // Console.WriteLine($"A: ({j}) {content[i][0]}-{content[i][1]}");
                }
                if ((js[1..] + js[..^1]).Contains(js))
                {
                    partTwo += j;
                    // Console.WriteLine($"B: ({j}) {content[i][0]}-{content[i][1]}");
                }
            }
        }
        partOneS = partOne;
        partTwoS = partTwo;
    }
}