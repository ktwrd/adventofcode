using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(12)]
public class DayTwelve : IDayHandler
{
    public void Run(string[] inputData)
    {
        var grid = AdventHelper.ParseGrid(inputData);
        var a = PartOne(grid);
        Console.WriteLine($"Part One: {a}");
        _scanned.Clear();
        var b = PartTwo(grid);
        Console.WriteLine($"Part Two: {b}");
    }

    private List<Point> _scanned = [];
    private long PartOne(char[,] grid)
    {
        long result = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (_scanned.Contains(new(x, y)))
                {
                    continue;
                }

                long totalArea = 0;
                long totalBorder = 0;
                var queue = new Queue<Point>();
                queue.Enqueue(new Point(x, y));
                while (queue.Count > 0)
                {
                    var (a, b) = ExploreSection(grid, grid[x, y], queue);
                    totalArea += a;
                    totalBorder += b;
                }

                result += totalArea * totalBorder;
            }
        }

        return result;
    }


    private long PartTwo(char[,] grid)
    {
        long result = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (_scanned.Contains(new(x, y)))
                    continue;

                long totalArea = 0;
                var queue = new Queue<Point>();
                queue.Enqueue(new Point(x, y));
                var edges = new List<(Point, EdgeDirection)>();
                while (queue.Count > 0)
                {
                    var a = ExploreSectionPartTwo(grid, grid[x, y], queue, edges);
                    totalArea += a;
                }

                var sides = FindSidesPartTwo(edges);
                result += totalArea * sides;
            }
        }
        return result;
    }

    private long ExploreSectionPartTwo(
        char[,] grid,
        char sectionIdent,
        Queue<Point> queue,
        List<(Point, EdgeDirection)> edges)
    {
        var current = queue.Dequeue();
        if (_scanned.Contains(current))
            return 0;
        _scanned.Add(current);

        var check = new List<(Point, EdgeDirection)>()
        {
            (new(0, -1), EdgeDirection.Up),
            (new(0, 1), EdgeDirection.Down),
            (new(-1, 0), EdgeDirection.Left),
            (new(1, 0), EdgeDirection.Right),
        };
        foreach (var (o, d) in check)
        {
            CheckPointPartTwo(grid, sectionIdent, current, o, queue, edges, d);
        }

        return 1;
    }

    public long FindSidesPartTwo(List<(Point, EdgeDirection)> edges)
    {
        var sides = new List<(Point, EdgeDirection)>(edges);
        foreach (var (edgePoint, edgeDirection) in edges)
        {
            if (!sides.Contains((edgePoint, edgeDirection)))
            {
                continue;
            }

            if (edgeDirection == EdgeDirection.Up || edgeDirection == EdgeDirection.Down)
            {
                sides = PruneEdges(edgePoint, edgeDirection, new Point(-1, 0), sides);
                sides = PruneEdges(edgePoint, edgeDirection, new Point(1, 0), sides);
            }

            if (edgeDirection == EdgeDirection.Left || edgeDirection == EdgeDirection.Right)
            {
                sides = PruneEdges(edgePoint, edgeDirection, new Point(0, 1), sides);
                sides = PruneEdges(edgePoint, edgeDirection, new Point(0, -1), sides);
            }
        }

        return sides.LongCount();
    }

    private List<(Point, EdgeDirection)> PruneEdges(
        Point edgePoint,
        EdgeDirection edgeDirection,
        Point offset,
        List<(Point, EdgeDirection)> sides)
    {
        var result = new List<(Point, EdgeDirection)>(sides);
        var x = edgePoint.X + offset.X;
        var y = edgePoint.Y + offset.Y;

        bool ShouldContinue()
        {
            return result.Any(e => e.Item1.X == x && e.Item1.Y == y && e.Item2 == edgeDirection);
        }
        while (ShouldContinue())
        {
            result.RemoveAll(e => e.Item1.X == x && e.Item1.Y == y && e.Item2 == edgeDirection);
            x += offset.X;
            y += offset.Y;
        }

        return result;
    }

    private void CheckPointPartTwo(
        char[,] grid,
        char sectionIdent,
        Point point,
        Point offset,
        Queue<Point> queue,
        List<(Point, EdgeDirection)> edges,
        EdgeDirection direction)
    {
        var pos = new Point(point.X + offset.X, point.Y + offset.Y);
        var other = AdventHelper.InsideBounds(grid, pos)
            ? grid[pos.X, pos.Y]
            : ' ';
        if (other != sectionIdent)
        {
            edges.Add((point, direction));
            return;
        }

        if (!_scanned.Contains(pos))
        {
            queue.Enqueue(pos);
        }
    }

    public enum EdgeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private (long area, long border) ExploreSection(char[,] grid, char sectionIdent, Queue<Point> queue)
    {
        long border = 0;
        var current = queue.Dequeue();
        if (_scanned.Contains(current))
            return (0, 0);
        
        _scanned.Add(current);
        border += CheckPoint(grid, sectionIdent, new(current.X + 1, current.Y), queue);
        border += CheckPoint(grid, sectionIdent, new(current.X - 1, current.Y), queue);
        border += CheckPoint(grid, sectionIdent, new(current.X, current.Y + 1), queue);
        border += CheckPoint(grid, sectionIdent, new(current.X, current.Y - 1), queue);
        return (1, border);
    }

    private long CheckPoint(char[,] grid, char sectionIdent, Point point, Queue<Point> queue)
    {
        var other = AdventHelper.InsideBounds(grid, point)
            ? grid[point.X, point.Y]
            : ' ';
        if (other != sectionIdent)
        {
            return 1;
        }

        if (!_scanned.Contains(point))
        {
            queue.Enqueue(point);
        }

        return 0;
    }
}