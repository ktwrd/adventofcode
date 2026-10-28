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
            var s = long.Parse(content[i][..o]);
            var e = long.Parse(content[i][(o + 1)..]);
            for (long j = s; j <= e; j++)
            {
                var js = j.ToString();
                var half = js.Length / 2;
                if (js[half..] == js[..half])
                {
                    partOne += j;
                }
                if ((js[1..] + js[..^1]).Contains(js, StringComparison.Ordinal))
                {
                    partTwo += j;
                }
            }
        }
        partOneS = partOne;
        partTwoS = partTwo;
    }
}