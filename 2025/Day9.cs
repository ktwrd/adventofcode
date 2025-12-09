using System.Collections.Frozen;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 9)]
public class Day9 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var points = new QPoint[content.Length];
        long? p1best = null;
        long? p2best = null;
        for (int i = 0; i < points.Length; i++)
        {
            var s = content[i].Split(',');
            points[i] = new(int.Parse(s[0]), int.Parse(s[1]));
        }
        var pointsEdges = points.GetEdges();
        foreach (var items in points.SelectMany(ip => points
                .Where(jp => jp != ip)
                .Select(jp =>
                {
                    var topLeft = new QPoint(
                        Math.Min(ip.X, jp.X),
                        Math.Min(ip.Y, jp.Y));
                    var bottomRight = new QPoint(
                        Math.Max(ip.X, jp.X),
                        Math.Max(ip.Y, jp.Y)) + 1;
                    var width =  bottomRight.X - topLeft.X;
                    var height = bottomRight.Y - topLeft.Y;
                    return new
                    {
                        Area = (long)height * width,
                        Edges = new QDRectangle(topLeft + 0.5f, bottomRight - 1.5f).GetEdges()
                    };
                })))
        {
            if (p1best.GetValueOrDefault(-1) < items.Area)
            {
                p1best = items.Area;
            }
            if (!items.Edges.Any(pointsEdges.PolygonIntersects))
            {
                if (p2best.GetValueOrDefault(-1) < items.Area)
                {
                    p2best = items.Area;
                }
            }
        }

        partOne = p1best ?? 0;
        partTwo = p2best ?? 0;
    }
}