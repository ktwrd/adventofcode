namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 7)]
public class Day7 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var width = content[0].Length;
        var ret = 0;
        var startX = content[0].IndexOf('S');
        var p1b = new HashSet<int>()
        {
            startX
        };
        var p2b = new long[width];
        p2b[startX] = 1;

        for (int y = 0; y < content.Length; y++)
        {
            var b = new HashSet<int>();
            foreach (var i in p1b)
            {
                var c = content[y][i];
                if (c == '^')
                {
                    b.Add(i - 1);
                    b.Add(i + 1);
                    ret++;
                } else b.Add(i);
            }
            p1b = b;

            var nb = new long[width];
            for (int i = 0; i < width; i++)
            {
                if (content[y][i] == '^')
                {
                    nb[i-1]+=p2b[i];
                    nb[i+1]+=p2b[i];
                } else nb[i] += p2b[i];
            }
            p2b = nb;
        }
        partOne = ret;
        partTwo = p2b.Sum();
    }
}