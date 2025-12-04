namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 4)]
public class Day4 : IDayHandler
{
    public void Run(string[] content)
    {
        var contentCharArray = new ReadOnlySpan<char[]>([..content.Select(e => e.ToCharArray())]);
        var partOne = PartOne(ref contentCharArray);
        var partTwo = PartTwo(ref contentCharArray);
        Console.WriteLine($"Part One: {partOne}");
        Console.WriteLine($"Part Two: {partTwo}");
    }

    private static int PartOne(
        ref ReadOnlySpan<char[]> content)
    {
        int result = 0;
        for (int x = 0; x < content[0].Length; x++)
        {
            for (int y = 0; y < content.Length; y++)
            {
                if (content[y][x] == at)
                {
                    var posArrRollCount = GetAdjRollCount(ref content, ref x, ref y);
                    result += posArrRollCount < 4 ? 1 : 0;
                }
            }
        }
        return result;
    }

    private static int PartTwo(
        ref ReadOnlySpan<char[]> content)
    {
        int removed = 0;
        var anyDeleted = true;
        var a = new HashSet<QPoint>();
        while (anyDeleted)
        {
            a.Clear();
            for (int x = 0; x < content[0].Length; x++)
            {
                for (int y = 0; y < content.Length; y++)
                {
                    if (content[y][x] == at)
                    {
                        var posArrRollCount = GetAdjRollCount(ref content, ref x, ref y);
                        if (posArrRollCount < 4)
                        {
                            a.Add(new(x,y));
                        }
                    }
                }
            }
            foreach (var p in a) content[p.y][p.x] = dot;
            anyDeleted = a.Count > 0;
            removed += a.Count;
        }
        return removed;
    }
    private static byte GetAdjRollCount(
        ref ReadOnlySpan<char[]> content,
        ref int x, ref int y)
    {
        byte count = 0;
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

            if (content[py][px] == at) count++;
        }
        return count;
    }

    private const char at = '@';
    private const char dot = '.';
}