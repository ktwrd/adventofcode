using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 2)]
public class Day2 : IDayHandler
{
    public void Run(string[] contentL, out object partOneS, out object partTwoS)
    {
        var content = contentL[0].Trim().Split(',');
        long partOne = 0;
        long partTwo = 0;
        for (int i = 0; i < content.Length; i++)
        {
            var o = content[i].IndexOf('-');
            var s = long.Parse(content[i].Substring(0, o));
            var e = long.Parse(content[i].Substring(o + 1));
            for (long j = s; j <= e; j++)
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