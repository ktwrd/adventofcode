using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(20)]
public class Day20 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(string[] inputData)
    {
        var d = Parse(inputData);
        var f = Find(d);
        return PartOneSolve(f);
    }

    public long PartTwo(string[] inputData)
    {
        var d = Parse(inputData);
        var f = Find(d);
        return PartTwoSolve(f);
    }

    private Point _start;
    private Point _end;

    public MazeElement[,] Parse(string[] inputData)
    {
        int width = inputData[0].Length;
        int height = inputData.Length;
        var result = new MazeElement[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = inputData[y].ToCharArray();
            for (int x = 0; x < width; x++)
            {
                result[x, y] = row[x] == '#' ? MazeElement.Wall
                    : MazeElement.Empty;
                if (row[x] == 'S')
                {
                    _start = new(x, y);
                }
                else if (row[x] == 'E')
                {
                    _end = new(x, y);
                }
            }
        }

        return result;
    }
    public enum MazeElement
    {
        Empty,
        Wall,
    }

    public List<Point> GetNeighbors(MazeElement[,] maze, Point current, Func<Point, MazeElement, bool> condition)
    {
        var data = new List<Point>()
        {
            new(0, 1),
            new(0, -1),
            new(1, 0),
            new(-1, 0),

            new(1, 1),
            new(1, -1),
            new(-1, 1),
            new(-1, -1)
        }
        .Select(e => new Point(e.X + current.X, e.Y + current.Y))
        .Where(e => AdventHelper.InsideBounds(maze, e));

        return data.Where(e => condition(e, maze[e.X, e.Y])).ToList();
    }
    
    public List<Point> Find(MazeElement[,] maze)
    {
        var pathPositions = new List<Point>();
        var visited = new HashSet<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(_start);

        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            pathPositions.Add(p);

            if (p.X == _end.X && p.Y == _end.Y)
            {
                break;
            }

            visited.Add(p);

            var neigh = GetNeighbors(maze, p, (a, b) =>
                b != MazeElement.Wall && !visited.Contains(a));
            if (neigh.Count > 0)
            {
                queue.Enqueue(neigh[0]);
            }
        }

        return pathPositions;
    }

    public long PartOneSolve(List<Point> pathPositions)
    {
        long skips = 0;
        var savedData = new Dictionary<int, int>();
        for (int i = 0; i < pathPositions.Count - 1; i++)
        {
            for (int j = i + 1; j < pathPositions.Count; j++)
            {
                var savedBySkipping = j - i;

                if (pathPositions[i].X == pathPositions[j].X || pathPositions[i].Y == pathPositions[j].Y)
                {
                    var diffX = Math.Abs(pathPositions[i].X - pathPositions[j].X);
                    var diffY = Math.Abs(pathPositions[i].Y - pathPositions[j].Y);
                    if (diffX + diffY <= 2)
                    {
                        var saved = savedBySkipping - (diffX + diffY);
                        if (savedData.ContainsKey(saved))
                        {
                            savedData[saved]++;
                        }
                        else
                        {
                            savedData[saved] = 1;
                        }
                        if (saved >= 100)
                        {
                            skips++;
                        }
                    }
                }
            }
        }

        return skips;
    }

    public long PartTwoSolve(List<Point> pathPositions)
    {
        long skips = 0;
        var savedData = new Dictionary<long, long>();
        for (int i = 0; i < pathPositions.Count - 1; i++)
        {
            for (int j = i + 1; j < pathPositions.Count; j++)
            {
                var savedBySkipping = j - i;

                    var diffX = Math.Abs(pathPositions[i].X - pathPositions[j].X);
                    var diffY = Math.Abs(pathPositions[i].Y - pathPositions[j].Y);
                    if (diffX + diffY <= 20)
                    {
                        var saved = savedBySkipping - (diffX + diffY);
                        if (saved >= 100)
                        {
                            skips++;
                            
                            if (savedData.ContainsKey(saved))
                            {
                                savedData[saved]++;
                            }
                            else
                            {
                                savedData[saved] = 1;
                            }
                        }
                    }
            }
        }

        return skips;
    }
}










