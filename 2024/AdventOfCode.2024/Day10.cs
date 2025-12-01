using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(10)]
public class Day10 : IDayHandler
{
    public void Run(string[] lines)
    {
        var a = PartOne(lines);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(lines);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(string[] inputData)
    {
        var map = ParseMap(inputData);
        long result = 0;
        foreach (var p in GetStartPoints(map))
        {
            result += FindPartOne(map, p);
        }

        return result;
    }

    public long PartTwo(string[] inputData)
    {
        var map = ParseMap(inputData);
        long result = 0;
        foreach (var p in GetStartPoints(map))
        {
            result += FindPartTwo(map, p, []);
        }

        return result;
    }

    public List<Point> GetStartPoints(int[,] map)
    {
        var result = new List<Point>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == 0)
                {
                    result.Add(new Point(x,y));
                }
            }
        }
        return result;
    }
    
    public int[,] ParseMap(string[] lines)
    {
        int width = lines[0].Length;
        int height = lines.Length;

        var result = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = lines[y].ToCharArray();
            for (int x = 0; x < width; x++)
                result[x, y] = int.Parse(row[x].ToString());
        }

        return result;
    }

    public long FindPartOne(int[,] map, Point start)
    {
        var visited = new HashSet<Point>();
        var reachable = new HashSet<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentHeight = map[current.X, current.Y];
            if (currentHeight == 9)
            {
                reachable.Add(current);
            }

            foreach (var (offsetX, offsetY) in Directions)
            {
                var newX = current.X + offsetX;
                var newY = current.Y + offsetY;

                if (!WithinBounds(map, newX, newY))
                    continue;

                var newP = new Point(newX, newY);
                if (!visited.Contains(newP) && map[newP.X, newP.Y] == currentHeight + 1)
                {
                    visited.Add(newP);
                    queue.Enqueue(newP);
                }
            }
        }
        return reachable.Count;
    }

    private static (int, int)[] Directions =>
    [
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0)
    ];

    public long FindPartTwo(int[,] map, Point start, List<Point> visited)
    {
        var currentHeight = map[start.X, start.Y];
        if (currentHeight == 9)
            return 1;

        long paths = 0;

        visited.Add(start);
        foreach (var (offsetX, offsetY) in Directions)
        {
            var newX = start.X + offsetX;
            var newY = start.Y + offsetY;

            if (!WithinBounds(map, newX, newY))
                continue;

            var newP = new Point(newX, newY);
            var newH = map[newP.X, newP.Y];
            if (!visited.Contains(newP) && newH == currentHeight + 1)
            {
                paths += FindPartTwo(map, newP, [..visited]);
            }
        }
        
        return paths;
    }

    public bool CanStep(int[,] map, int x, int y, int previous)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        if (x < 0 || x > width - 1)
            return false;
        if (y < 0 || y > height - 1)
            return false;

        return map[x, y] == previous + 1;
    }

    public bool WithinBounds(int[,] map, int x, int y)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        if (x < 0 || x > width - 1)
            return false;
        if (y < 0 || y > height - 1)
            return false;

        return true;
    }
}