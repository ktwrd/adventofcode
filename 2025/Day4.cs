namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 4)]
public class Day4 : IDayHandler
{
    public void Run(string[] contentR, out object partOneS, out object partTwoS)
    {
        var width = contentR[0].Length;
        var height = contentR.Length;
        var memory = new bool[contentR[0].Length, contentR.Length];
        for (int y = 0; y < contentR.Length; y++)
        {
            for (int x = 0; x < contentR[0].Length; x++)
                memory[x, y] = contentR[y][x] == at;
        }
        short partOne = 0;
        short partTwo = 0;
        var anyDeleted = true;
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
                    // if (content[y][x] == at)
                    if (memory[x, y])
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

                            if (memory[px, py]) rollCount++;
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
                    memory[x, y] = false;
                    toDot[xa,y] = 0;
                    ac++;
                }
            }
            anyDeleted = ac > 0;
            partTwo += ac;
            doingPartOne = false;
        }
        partOneS = partOne;
        partTwoS = partTwo;
    }

    private const char at = '@';
    private const char dot = '.';
}