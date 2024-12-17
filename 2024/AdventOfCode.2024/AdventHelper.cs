using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

public static class AdventHelper
{
    public static bool InsideBounds<T>(T[,] grid, Point position)
    {
        return InsideBounds(grid, position.X, position.Y);
    }
    public static bool InsideBounds<T>(T[,] grid, int x, int y)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        if (x < 0 || x > width - 1)
            return false;
        if (y < 0 || y > height - 1)
            return false;

        return true;
    }

    public static double GetDistance(Point p1, Point p2)
    {
        return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
    }
    public static double GetDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
    }
    public static char[,] GenerateGrid(int width, int height, char value)
    {
        var result = new char[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = value;
            }
        }

        return result;
    }

    public static long[,] GenerateGrid(int width, int height, long value)
    {
        var result = new long[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = value;
            }
        }

        return result;
    }

    public static int[,] GenerateGrid(int width, int height, int value)
    {
        var result = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                result[x, y] = value;
            }
        }

        return result;
    }

    public static char[,] ParseGrid(string[] inputData)
    {
        var width = inputData[0].Length;
        var height = inputData.Length;
        var grid = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = inputData[y].ToCharArray();
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = row[x];
            }
        }

        return grid;
    }

    public static int[,] ParseNumberGrid(string[] inputData)
    {
        int width = inputData[0].Length;
        int height = inputData.Length;

        var result = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = inputData[y].ToCharArray();
            for (int x = 0; x < width; x++)
            {
                result[x, y] = int.Parse(row[x].ToString());
            }
        }

        return result;
    }

    public static void PrintGrid(char[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(grid[x, y]);
            }
            Console.Write(Environment.NewLine);
        }
    }

    public static void PrintGrid(int[,] grid)
    {
        int max = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var l = grid[x, y].ToString().Length;
                if (l > max) max = l;
            }
        }

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                var p = grid[x, y];
                if (p == 0)
                {
                    Console.Write(".".PadRight(max + 1, ' '));
                }
                else
                {
                    Console.Write(p.ToString().PadRight(max + 1, ' '));
                }
            }
            Console.Write("\n");
        }
    }

    public static void PrintGrid(long[,] grid)
    {
        int max = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var l = grid[x, y].ToString().Length;
                if (l > max) max = l;
            }
        }

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                var p = grid[x, y];
                if (p == 0)
                {
                    Console.Write(".".PadRight(max + 1, ' '));
                }
                else
                {
                    Console.Write(p.ToString().PadRight(max + 1, ' '));
                }
            }
            Console.Write("\n");
        }
    }

    public static long Pow(long x, long count)
    {
        long result = x;
        for (long i = 0; i < count; i++)
            result *= x;
        return result;
    }
}