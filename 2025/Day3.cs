namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 3)]
public class Day3 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        partOne = Calc(ref content, 2);
        partTwo = Calc(ref content, 12);
    }
    private static long Calc(ref string[] content, int k)
    {
        long v = 0;
        foreach (var line in content)
        {
            var s = new List<char>();
            var r = line.Length - k;
            foreach (var c in line)
            {
                while (r > 0 && s.Count > 0 && s[^1] < c)
                {
                    s.RemoveAt(s.Count - 1);
                    r--;
                }
                s.Add(c);
            }
            v += long.Parse(new string(s.Take(k).ToArray()));
        }
        return v;
    }
}