namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 3)]
public class Day3 : IDayHandler
{
    public void Run(string[] content)
    {
        var part1 = Calc(ref content, 2);
        var part2 = Calc(ref content, 12);
        Console.WriteLine($"Part One: {part1}");
        Console.WriteLine($"Part Two: {part2}");
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