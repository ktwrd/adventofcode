using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 2)]
public class Day2 : IDayHandler
{
    public void Run(string[] contentL)
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
        long ptA = 0;
        long ptB = 0;
        for (int i = 0; i < content.Length; i++)
        {
            for (long j = content[i][0]; j <= content[i][1]; j++)
            {
                var js = j.ToString();
                var half = js.Length / 2;
                if (js[half..] == js[..half])
                {
                    ptA += j;
                    // Console.WriteLine($"A: ({j}) {content[i][0]}-{content[i][1]}");
                }
                if ((js[1..] + js[..^1]).Contains(js))
                {
                    ptB += j;
                    // Console.WriteLine($"B: ({j}) {content[i][0]}-{content[i][1]}");
                }


            }
        }
        Console.WriteLine($"Part A: {ptA}");
        Console.WriteLine($"Part B: {ptB}");
    }
}