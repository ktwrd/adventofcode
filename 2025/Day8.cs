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
            var a = points[i];
            for (var j = i + 1; j < points.Length; j++)
            {
                var b = points[j];
                distanceDict[a.StraightLineDistanceL(b)].Add((a, b));
            }
        }
        partOne = PartOne(ref distanceDict, ref points);
        partTwo = PartTwo(ref distanceDict, ref points);
    }

    private static long PartOne(
        ref DefaultDictionary<long, HashSet<(QVector3 From, QVector3 To)>> distanceDict,
        ref QVector3[] points)
    {
        var set = new DisjointSet<QVector3>(points);
        var sizes = new DefaultDictionary<QVector3, int>(defaultValue: 0);
        var connected = 0;
        
        foreach (var distance in distanceDict.Keys.OrderBy(d => d))
        {
            foreach (var (a, b) in distanceDict[distance])
            {
                set.Union(a, b);
                if (++connected >= 1000)
                {
                    return Finish(ref points);
                }
            }
        }
        
        return Finish(ref points);

        long Finish(ref QVector3[] points)
        {
            foreach (var box in points)
            {
                sizes[set.FindSet(box)]++;
            }
            return sizes.Values
                .OrderByDescending(size => size)
                .Take(3)
                .Aggregate(seed: 1L, func: (acc, size) => acc * size);
        }
    }
    private static long PartTwo(
        ref DefaultDictionary<long, HashSet<(QVector3 From, QVector3 To)>> distanceDict,
        ref QVector3[] points)
    {
        var djs = new DisjointSet<QVector3>(points);
        var success = 0;
        var requiredUnions = points.Length - 1;
        
        foreach (var distance in distanceDict.Keys.OrderBy(e => e))
        {
            foreach (var (a, b) in distanceDict[distance])
            {
                if (djs.Union(a, b) && ++success == requiredUnions)
                {
                    return (long)a.X * b.X;
                }
            }
        }
        throw new InvalidOperationException();
    }
}