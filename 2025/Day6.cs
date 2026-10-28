using System.Text;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 6)]
public class Day6 : IDayHandler
{
    const char cSpace = ' ';
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var operations = content[^1].Split(cSpace)
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Select(e => e.ToCharArray()[0])
            .ToArray();
        var rows = content
            .Take(content.Length - 1)
            .Select(x => x.Split(cSpace)
                .Where(e => e.Trim().Length > 0)
                .Select(long.Parse)
                .ToArray())
            .ToArray();
        var width = operations.Length;
        var height = rows.Length;
        partOne = Calculate(ref operations, ref rows);

        var verticalValues = new long[content[0].Length];

        var s = new StringBuilder(16);
        for (int x = 0; x < content[0].Length; x++)
        {
            for (int y = 0; y < rows.Length; y++)
            {
                if (content[y][x] != cSpace) s.Append(content[y][x]);
            }
            if (s.Length < 1)
            {
                verticalValues[x] = -1;            
            }
            else
            {
                verticalValues[x] = long.Parse(s.ToString());
                s.Clear();
            }
        }

        long xx = 0;
        long yy = 0;
        var n1 = 0;
        var grid = new long[rows[0].Length, rows.Length];
        var stack = new List<long>(10);
        for (int i = verticalValues.Length - 1; i >= -1 ; i--)
        {
            if (i == -1 || (i >= 0 && verticalValues[i] == -1))
            {
                if (stack.Count < 1) continue;
                for (int p = stack.Count - 1; p >= 0 ; p--)
                {
                    grid[xx, yy] = stack[p];
                    yy++;
                }
                stack.Clear();

                n1++;
                yy = 0;
                xx++;
            }
            else
            {
                stack.Add(verticalValues[i]);
            }
        }

        partTwo = Calculate(ref operations, ref grid, rows[0].Length, rows.Length);
    }

    private static long Calculate(ref char[] operations, ref long[][] rows)
    {
        var result = new long[operations.Length];
        for (int x = 0; x < operations.Length; x++)
        {
            for (int y = 0; y < rows.Length; y++)
            {
                if (y == 0)
                {
                    result[x] = rows[y][x];
                }
                else
                {
                    switch (operations[x])
                    {
                        case '*':
                            result[x] *= rows[y][x];
                            break;
                        case '+':
                            result[x] += rows[y][x];
                            break;
                    }
                }
            }
        }
        return result.Sum();
    }
    private static long Calculate(ref char[] operations, ref long[,] grid, int w, int h)
    {
        long r = 0;
        for (int x = 0; x < w; x++)
        {
            long p = 0;
            for (int y = 0; y < h; y++)
            {
                if (grid[x,y] == 0) continue;
                if (y == 0)
                {
                    p = grid[x,y];
                }
                else
                {
                    switch (operations[x])
                    {
                        case '*':
                            p *= grid[x,y];
                            break;
                        case '+':
                            p += grid[x,y];
                            break;
                    }
                }
            }
            r += p;
        }
        return r;
    }
}