using System.Diagnostics;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 6)]
public class Day6 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var operations = content[content.Length - 1].Split(' ')
            .Select(e => e.Trim())
            .Where(e => !string.IsNullOrEmpty(e))
            .Select(e => e.ToCharArray()[0])
            .ToArray();
        var rows = content.Take(content.Length - 1)
            .Select(x => x.Split(' ')
            .Select(e => e.Trim())
            .Where(e => !string.IsNullOrEmpty(e))
            .Select(long.Parse)
            .ToArray())
            .ToArray();
        var width = operations.Length;
        var height = rows.Length;
        partOne = Calculate(operations, rows);

        var verticalValues = new long[content[0].Length];

        for (int x = 0; x < content[0].Length; x++)
        {
            var valueBuilder = "";
            for (int y = 0; y < rows.Length; y++)
            {
                if (content[y][x] != ' ') valueBuilder += content[y][x].ToString();
            }
            if (valueBuilder == "")
            {
                verticalValues[x] = -1;            
            }
            else
            {
                verticalValues[x] = long.Parse(string.Join("", valueBuilder));
            }
        }

        int xx = 0;
        int yy = 0;
        var n1 = 0;
        var grid = new long[rows[0].Length, rows.Length];
        var stack = new List<long>(10);
        foreach (var v in verticalValues)
        {
            if (v == -1)
            {
                ShiftStack();
            }
            else
            {
                stack.Add(v);
            }
        }
        ShiftStack();
        void ShiftStack()
        {
            if (stack.Count < 1) return;
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

        partTwo = Calculate(operations, grid, rows[0].Length, rows.Length);
    }

    private long Calculate(char[] operations, long[][] rows)
    {
        var result = new long[operations.Length];
        for (int x = 0; x < operations.Length; x++)
        {
            var ope = operations[x];
            for (int y = 0; y < rows.Length; y++)
            {
                if (y == 0)
                {
                    result[x] = rows[y][x];
                }
                else
                {
                    switch (ope)
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
    private long Calculate(char[] operations, long[,] grid, int w, int h)
    {
        var result = new long[w];
        for (int x = 0; x < w; x++)
        {
            var ope = operations[x];
            for (int y = 0; y < h; y++)
            {
                if (grid[x,y] == 0) continue;
                if (y == 0)
                {
                    result[x] = grid[x,y];
                }
                else
                {
                    switch (ope)
                    {
                        case '*':
                            result[x] *= grid[x,y];
                            break;
                        case '+':
                            result[x] += grid[x,y];
                            break;
                    }
                }
            }
        }
        return result.Sum();
    }
}