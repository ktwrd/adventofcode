namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 4)]
public class Day4 : IDayHandler
{
    public void Run(string[] content)
    {
        var contentCharArray = content.Select(e => e.ToCharArray()).ToArray();
        var partOne = PartOne(ref contentCharArray);
        Console.WriteLine($"Part One: {partOne}");
        var partTwo = PartTwo(ref contentCharArray);
        Console.WriteLine($"Part Two: {partTwo}");
    }

    private static long PartOne(
        ref char[][] content)
    {
        var last = new QPoint(content[0].Length - 1, content.Length - 1);
        long result = 0;
        for (int x = 0; x < content[0].Length; x++)
        {
            for (int y = 0; y < content.Length; y++)
            {
                if (content[y][x] == '@')
                {
                    var posArrRollCount = GetAdjRollCount(ref content, ref x, ref y, ref last);
                    result += posArrRollCount < 4 ? 1 : 0;
                }
            }
        }
        return result;
    }

    private static long PartTwo(
        ref char[][] content)
    {
        var last = new QPoint(content[0].Length - 1, content.Length - 1);
        long removed = 0;
        var anyDeleted = true;
        while (anyDeleted)
        {
            anyDeleted = false;
            var toDelete = new HashSet<QPoint>(4);
            for (int x = 0; x < content[0].Length; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    if (content[y][x] == '@')
                    {
                        var posArrRollCount = GetAdjRollCount(ref content, ref x, ref y, ref last);
                        if (posArrRollCount < 4)
                        {
                            removed++;
                            toDelete.Add(new(x, y));
                            anyDeleted = true;
                        }
                    }
                }
            }
            foreach (var p in toDelete)
            {
                content[p.y][p.x] = '.';
            }
            toDelete.Clear();
        }
        return removed;
    }

    private static byte GetAdjRollCount(
        ref char[][] content,
        ref int x, ref int y,
        ref QPoint last)
    {
        var posArr = new QPoint[]
        {
            new(x - 1, y - 1), new(x, y - 1), new(x + 1, y - 1),
            new(x - 1, y),                    new(x + 1, y),
            new(x - 1, y + 1), new(x, y + 1), new(x + 1, y + 1)
        };
        byte posArrRollCount = 0;
        foreach (var pos in posArr)
        {
            if (pos.x < 0 || pos.x > last.x
                || pos.y < 0 || pos.y > last.y) continue;
            if (content[pos.y][pos.x] == '@')
                posArrRollCount++;
        }
        return posArrRollCount;
    }
}