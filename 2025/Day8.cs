namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 8)]
public class Day8 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var points = content.Select(QVector3.Parse).ToArray();

        var distanceDict = new DefaultDictionary<long, HashSet<(QVector3 From, QVector3 To)>>(
            defaultSelector: _ => [],
            defaultCapacity: 1_000
        );
        for (var i = 0; i < points.Length - 1; i++)
        {
            var a = points.ElementAt(i);
            for (var j = i + 1; j < points.Length; j++)
            {
                var b = points.ElementAt(j);
                distanceDict[a.StraightLineDistanceL(b)].Add((a, b));
            }
        }
        var distanceDictKeys = distanceDict.Keys.OrderBy(e => e).ToArray();
        var calcSet = new DisjointSet<QVector3>(points);
        var partOneSizes = new DefaultDictionary<QVector3, int>(defaultValue: 0, defaultCapacity: points.Length);
        var partOneConnected = 0;
        var partTwoSuccess = 0;
        var requiredUnions = points.Length - 1;

        var doingPartOne = true;
        var doingPartTwo = true;
        long partOneValue = 0;
        long partTwoValue = 0;

        foreach (var distance in distanceDictKeys)
        {
            foreach (var (a, b) in distanceDict[distance])
            {
                var r = calcSet.Union(a, b);
                if (doingPartOne && ++partOneConnected >= 1000)
                {
                    partOneValue = FinishPartOne(ref partOneSizes, ref calcSet, ref points);
                    doingPartOne = false;
                }
                if (doingPartTwo && r && ++partTwoSuccess == requiredUnions)
                {
                    partTwoValue = (long)a.X * b.X;
                    doingPartTwo = false;
                }
                if (!doingPartOne && !doingPartTwo) break;
            }
            if (!doingPartOne && !doingPartTwo) break;
        }
        
        if (doingPartOne)
        {
            partOneValue = FinishPartOne(ref partOneSizes, ref calcSet, ref points);
        }
        if (doingPartTwo)
        {
            throw new InvalidOperationException();
        }
        partOne = partOneValue;
        partTwo = partTwoValue;

    }
    private static long FinishPartOne(
        ref DefaultDictionary<QVector3, int> sizes,
        ref DisjointSet<QVector3> set,
        ref QVector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            sizes[set.FindSet(points[i])]++;
        }
        return sizes.Values
            .OrderByDescending(size => size)
            .Take(3)
            .Aggregate(seed: 1L, func: (acc, size) => acc * size);
    }
}