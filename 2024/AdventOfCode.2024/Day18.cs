using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(18)]
public class Day18 : IDayHandler
{
    public enum MemoryObject
    {
        Free,
        Corrupt,
    }
    public void Run(string[] inputLines)
    {
        var a = PartOne(inputLines);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputLines);
        Console.WriteLine($"Part Two: {b}");
    }

    private MemoryObject[,] ParseData(string[] inputData, int max)
    {
        var maze = new MemoryObject[71, 71];
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = MemoryObject.Free;
            }
        }

        for (int i = 0; i < max; i++)
        {
            var spl = inputData[i].Split(',');
            var xp = int.Parse(spl[0]);
            var yp = int.Parse(spl[1]);
            maze[xp, yp] = MemoryObject.Corrupt;
        }

        return maze;
    }
    private long Find(string[] inputData, int max)
    {
        var maze = ParseData(inputData, max);
        var seen = new HashSet<Point>();
        var queue = new Queue<(long, Point)>();
        queue.Enqueue((0, new(0, 0)));

        while (queue.Count > 0)
        {
            var (c, pos) = queue.Dequeue();
            if (pos.X == 70 && pos.Y == 70)
            {
                return c;
            }

            foreach (var d in Directions)
            {
                var np = new Point(pos.X + d.X, pos.Y + d.Y);
                if (AdventHelper.InsideBounds(maze, np))
                {
                    if (maze[np.X, np.Y] != MemoryObject.Corrupt && seen.Contains(np) == false)
                    {
                        queue.Enqueue((c + 1, np));
                        seen.Add(np);
                    }
                }
            }
        }

        return -1;
    }
    private long PartOne(string[] inputData)
    {
        return Find(inputData, 1024);
    }

    private string PartTwo(string[] inputData)
    {
        int length = 1024;
        var dataLength = inputData.Length;
        while (length < dataLength - 1)
        {
            var m = (length + dataLength) / 2;
            var x = Find(inputData, m);
            if (x == -1)
            {
                return inputData[m - 1];
            }
            length++;
        }

        return "i dunno";
    }

    public static ReadOnlyCollection<Point> Directions => new List<Point>()
    {
        new(0, 1),
        new(0, -1),
        new(1, 0),
        new(-1, 0)
    }.AsReadOnly();
}