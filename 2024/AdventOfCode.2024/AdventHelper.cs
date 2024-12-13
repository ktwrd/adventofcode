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
}