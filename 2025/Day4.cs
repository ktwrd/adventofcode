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
        var toDot = new byte[content[0].Length,content.Length];
        var doingPartOne = true;
        while (anyDeleted)
        {
            short ac = 0;
            for (int x = 0; x < content[0].Length; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    if (content[y][x] == at)
                    {
                        byte rollCount = 0;
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 4) continue;
                            int im = i % 3;
                            var ry = i < 3 ? -1 : i < 6 ? 0 : 1;
                            var py = ry + y;
                            if (py < 0 || py >= content.Length) continue;

                            var rx = im == 0 ? -1 : im == 2 ? 1 : 0;
                            var px = rx + x;
                            if (px < 0 || px >= content[0].Length) continue;

                            if (content[py][px] == at) rollCount++;
                        }
                        if (rollCount < 4)
                        {
                            if (toDot[x,y] == 0) ac++;
                            toDot[x,y] = 1;
                            if (doingPartOne)
                            {
                                partOne++;
                            }
                        }
                    }
                }
            }
            for (int x = 0; x < content[0].Length; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    if (toDot[x,y] != 0) content[y][x] = dot;
                    toDot[x,y] = 0;
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