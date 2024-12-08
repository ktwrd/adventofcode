using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(8)]
public class DayEight : IDayHandler
{
    public void Run(string[] input)
    {
        var (a, b) = PartOne(input);
        Console.WriteLine($"Part One: {a}");
        Console.WriteLine($"Part Two: {b}");
    }

    public (int, int) PartOne(string[] input)
    {
        var map = GetData(input);
        var nodes = FindNodes(map);
        int width = map.GetLength(0);
        int height = map.GetLength(1);


        var one = new HashSet<Point>();
        var two = new HashSet<Point>();
        foreach (var freq in nodes.Keys)
        {
            var freqLocList = nodes[freq];
            foreach (var freqLoc in freqLocList)
            {
                foreach (var freqLocOuter in freqLocList)
                {
                    if (freqLoc == freqLocOuter)
                        continue;
                    
                    var diffX = freqLoc.X - freqLocOuter.X;
                    var diffY = freqLoc.Y - freqLocOuter.Y;
                    
                    var freqLocOffset = new Point(freqLoc.X + diffX, freqLoc.Y + diffY);
                    
                    if (!OutOfBounds(freqLocOffset, width, height))
                    {
                        one.Add(freqLocOffset);
                    }

                    while (!OutOfBounds(freqLocOffset, width, height))
                    {
                        two.Add(freqLocOffset);
                        freqLocOffset = new Point(freqLocOffset.X + diffX, freqLocOffset.Y + diffY);
                    }

                    two.Add(freqLoc);
                    
                    var freqLocOuterOffsetB = new Point(freqLocOuter.X - diffX, freqLocOuter.Y - diffY);
                    
                    if (!OutOfBounds(freqLocOuterOffsetB, width, height))
                    {
                        one.Add(freqLocOuterOffsetB);
                    }

                    while (!OutOfBounds(freqLocOuterOffsetB, width, height))
                    {
                        two.Add(freqLocOuterOffsetB);
                        freqLocOuterOffsetB = new Point(freqLocOuterOffsetB.X - diffX, freqLocOuterOffsetB.Y - diffY);
                    }

                    two.Add(freqLocOuter);
                }
            }
        }

        return (one.Count, two.Count);
    }

    public bool OutOfBounds(Point point, int width, int height)
    {
        return point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height;
    }
    
    private char[,] GetData(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;

        var result = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result[x, y] = input[y][x];
            }
        }

        return result;
    }

    private Dictionary<char, List<Point>> FindNodes(char[,] data)
    {
        var result = new Dictionary<char, List<Point>>();
        int width = data.GetLength(0);
        int height = data.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var c = data[x, y];
                if (c == '.')
                    continue;
                if (result.ContainsKey(c) == false)
                    result[c] = [];
                result[c].Add(new Point(x,y ));
            }
        }

        return result;
    }
}