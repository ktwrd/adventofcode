using System.Collections.Frozen;
using System.Reflection.Metadata;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 4)]
public class Day4 : IDayHandler
{
    public void Run(string[] content)
    {
        var contentCharArray = new ReadOnlySpan<char[]>(content.Select(e => e.ToCharArray()).ToArray());
        var partOne = PartOne(ref contentCharArray);
        var partTwo = PartTwo(ref contentCharArray);
        Console.WriteLine($"Part One: {partOne}");
        Console.WriteLine($"Part Two: {partTwo}");
    }

    private static long PartOne(
        ref ReadOnlySpan<char[]> content)
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
        ref ReadOnlySpan<char[]> content)
    {
        var last = new QPoint(content[0].Length - 1, content.Length - 1);
        long removed = 0;
        var anyDeleted = true;
        var a = new HashSet<QPoint>();
        while (anyDeleted)
        {
            a.Clear();
            for (int x = 0; x < content[0].Length; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    if (content[y][x] == '@')
                    {
                        var posArrRollCount = GetAdjRollCount(ref content, ref x, ref y, ref last);
                        if (posArrRollCount < 4)
                        {
                            a.Add(new(x,y));
                        }
                    }
                }
            }
            foreach (var p in a) content[p.y][p.x] = '.';
            anyDeleted = a.Count > 0;
            removed += a.Count;
        }
        return removed;
    }

    private static readonly QPoint[] offsets =
    [
        new(-1, -1), new(0, -1), new(1, -1),
        new(-1, 0),              new(1, 0),
        new(-1, 1),  new(0, 1),  new(1, 1)
    ];

    private static byte GetAdjRollCount(
        ref ReadOnlySpan<char[]> content,
        ref int x, ref int y,
        ref QPoint last)
    {
        byte posArrRollCount = 0;
        foreach (var pos in offsets)
        {
            var px = pos.x + x;
            var py = pos.y + y;
            if (px < 0 || px > last.x
                || py < 0 || py > last.y) continue;
            if (content[py][px] == '@')
                posArrRollCount++;
        }
        return posArrRollCount;
    }
}