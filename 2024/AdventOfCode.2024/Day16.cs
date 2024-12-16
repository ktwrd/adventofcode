using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(16)]
public class DaySixteen : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    private void PrintMaze(MazeElement[,] maze)
    {
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                var p = maze[x, y];
                if (p == MazeElement.Wall)
                {
                    Console.Write('#');
                }
                else if (p == MazeElement.Start)
                {
                    Console.Write('S');
                }
                else if (p == MazeElement.End)
                {
                    Console.Write('E');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.Write("\n");
        }
    }
    
    public long PartOne(string[] inputData)
    {
        var maze = ParseMap(inputData);
        PrintMaze(maze);
        var (data, _) = Find(maze);
        var r = (from k in data.Keys where k.Item1 == _end select data[k]).ToList();
        var ordered = r.OrderBy(x => x).ToList();
        return ordered.First();
    }

    public (Dictionary<(Point position, Point direction), long> seen, List<Point> bestPath) Find(MazeElement[,] maze)
    {
        var bestScore = long.MaxValue;
        var bestPath = new List<Point>();
        var queue = new Queue<(Point position, Point direction, List<Point> path, long score)>();
        queue.Enqueue((_start, new(1, 0), [], 0));

        var seen = new Dictionary<(Point position, Point direction), long>();
        while (queue.Count > 0)
        {
            var (position, direction, path, score) = queue.Dequeue();
            if (seen.ContainsKey((position, direction)))
            {
                if (seen[(position, direction)] < score)
                {
                    continue;
                }
            }
            
            seen[(position, direction)] = score;

            if (position == _end)
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPath.Clear();
                    bestPath.AddRange(path);
                }
                else if (score == bestScore)
                {
                    bestPath.AddRange(path.Where(e => bestPath.Contains(e) == false));
                }

                continue;
            }
            
            var newpos = new Point(position.X + direction.X, position.Y + direction.Y);
            if (AdventHelper.InsideBounds(maze, newpos))
            {
                if (maze[newpos.X, newpos.Y] != MazeElement.Wall)
                {
                    queue.Enqueue((newpos, direction, [..path, position], score + 1));
                }
            }
            
            queue.Enqueue((position, new Point(direction.Y, -direction.X), path, score + 1000));
            queue.Enqueue((position, new Point(-direction.Y, direction.X), path, score + 1000));
        }

        return (seen, bestPath);
    }

    private Point _start = new(-1, -1);
    private Point _end = new(-1, -1);
    public long PartTwo(string[] inputData)
    {
        var maze = ParseMap(inputData);
        var (_, path) = Find(maze);
        return path.LongCount() + 1;
    }
    
    public MazeElement[,] ParseMap(string[] inputData)
    {
        var width = inputData[0].Length;
        var height = inputData.Length;
        var maze = new MazeElement[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = inputData[y].ToCharArray();
            for (int x = 0; x < width; x++)
            {
                maze[x, y] = MazeElement.Empty;
                if (CharToMazeElement.TryGetValue(row[x], out var v))
                {
                    maze[x, y] = v;
                }

                if (maze[x, y] == MazeElement.Start)
                {
                    _start = new Point(x, y);
                }
                else if (maze[x, y] == MazeElement.End)
                {
                    _end = new Point(x, y);
                }
            }
        }

        return maze;
    }

    public static ReadOnlyDictionary<char, MazeElement> CharToMazeElement => new Dictionary<char, MazeElement>()
    {
        {'#', MazeElement.Wall},
        {'.', MazeElement.Empty},
        {'S', MazeElement.Start},
        {'s', MazeElement.Start},
        {'E', MazeElement.End},
        {'e', MazeElement.End}
    }.AsReadOnly();
    public enum MazeElement
    {
        Wall,
        Empty,
        Start,
        End
    }
}