using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(14)]
public class Day14 : IDayHandler
{
    public void Run(string[] lines)
    {
        var a = PartOne(lines);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(lines);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(string[] lines)
    {
        var data = Solve(lines, 101, 103, 100);

        var width = data.GetLength(0);
        var height = data.GetLength(1);
        int halfWidth = Convert.ToInt32(Math.Floor(data.GetLength(0) / 2f));
        int halfHeight = Convert.ToInt32(Math.Floor(data.GetLength(1) / 2f));
        var quadrant = new long[4];
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                if (x >= 0
                    && x < halfWidth
                    && y >= 0
                    && y < halfHeight)
                {
                    quadrant[0] += data[x, y];
                }

                if (x >= halfWidth + 1
                    && x < width
                    && y >= 0
                    && y < halfHeight)
                {
                    quadrant[1] += data[x, y];
                }

                if (x >= 0
                    && x < halfWidth
                    && y >= halfHeight + 1
                    && y < height)
                {
                    quadrant[2] += data[x, y];
                }

                if (x >= halfWidth + 1
                    && x < width
                    && y >= halfHeight + 1
                    && y < height)
                {
                    quadrant[3] += data[x, y];
                }
            }
        }
        
        long result = 1;
        foreach (var t in quadrant)
        {
            result *= t;
        }
        return result;
    }

    public long[,] Solve(string[] lines, int width, int height, int iterations)
    {
        var robots = Parse(lines);
        var grid = AdventHelper.GenerateGrid(width, height, (long)0);
        foreach (var r in robots)
        {
            r.ApplyVelocity(width, height, iterations);
            r.EnsureIndex(grid);
        }
        return grid;
    }

    public List<Robot> Parse(string[] lines)
    {
        long index = 0;
        var result = new List<Robot>();
        foreach (var line in lines)
        {
            var split = line.Split(" ");
            Point position = Point.Empty;
            Point velocity = Point.Empty;
            foreach (var s in split)
            {
                var posIndex = s.IndexOf('p');
                var velocityIndex = s.IndexOf('v');

                Point ParseInner(int index)
                {
                    var l = s.Substring(index + 1);
                    var innerSplit = l.Split(",");
                    var x = innerSplit[0];
                    var y = innerSplit[1];
                    return new(int.Parse(x), int.Parse(y));
                }
                if (posIndex != -1)
                {
                    position = ParseInner(posIndex + 1);
                }
                else if (velocityIndex != -1)
                {
                    velocity = ParseInner(velocityIndex + 1);
                }
            }
            result.Add(new()
            {
                Index = index,
                Position = position,
                Velocity = velocity,
            });
            index++;
        }
        return result;
    }

    public long PartTwo(string[] lines)
    {
        Console.WriteLine($"[PartTwo] Calculating result (this may take a while)");
        int seconds = 0;
        var width = 101;
        var height = 103;
        while (true)
        {
            var data = Solve(lines, width, height, seconds);
            var bad = false;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bad = data[x, y] > 1;
                    if (bad)
                        break;
                }

                if (bad)
                    break;
            }

            if (bad == false)
            {
                return seconds;
            }

            seconds++;
        }
    }

    public struct Robot
    {
        public long Index;
        public Point Position;
        public Point Velocity;

        public void ApplyVelocity(int width, int height, int mul = 1)
        {
            if (mul == 0)
                return;
            var velocityX = mul <= 1 ? Velocity.X : Velocity.X * mul;
            var velocityY = mul <= 1 ? Velocity.Y : Velocity.Y * mul;
            var calc = new Point(
                (Position.X + velocityX) % width,
                (Position.Y + velocityY) % height);
            Position = new Point(
                calc.X < 0 ? width + calc.X : calc.X,
                calc.Y < 0 ? height + calc.Y : calc.Y);
        }

        public void EnsureIndex(long[,] grid)
        {
            grid[Position.X, Position.Y]++;
        }
    }
}