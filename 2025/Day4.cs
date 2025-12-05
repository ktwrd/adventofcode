namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 4)]
public class Day4 : IDayHandler
{
    public void Run(string[] contentR)
    {
        var content = contentR.Select(e => e.ToCharArray()).ToArray();
        short partOne = 0;
        short partTwo = 0;
        var anyDeleted = true;
        var width = content[0].Length;
        var height = content.Length;
        var toDot = new byte[(width/8) + 1,height];
        var doingPartOne = true;
        while (anyDeleted)
        {
            short ac = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xa = x / 8;
                    var xb = (byte)(1 << (x%8));
                    if (content[y][x] == at)
                    {
                        byte rollCount = 0;
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 4) continue;
                            int im = i % 3;
                            var ry = i < 3 ? -1 : i < 6 ? 0 : 1;
                            var py = ry + y;
                            if (py < 0 || py >= height) continue;

                            var rx = im == 0 ? -1 : im == 2 ? 1 : 0;
                            var px = rx + x;
                            if (px < 0 || px >= width) continue;

                            if (content[py][px] == at) rollCount++;
                        }
                        if (rollCount < 4)
                        {
                            toDot[xa,y] |= xb;
                            if (doingPartOne)
                            {
                                partOne++;
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xa = x / 8;
                    var xb = (byte)(1 << (x%8));
                    if ((toDot[xa,y] & xb) != xb) continue;
                    content[y][x] = dot;
                    toDot[xa,y] = 0;
                    ac++;
                }
            }
            anyDeleted = ac > 0;
            partTwo += ac;
            doingPartOne = false;
        }
        Console.WriteLine($"Part One: {partOne}");
        Console.WriteLine($"Part Two: {partTwo}");
    }

    private const char at = '@';
    private const char dot = '.';
}