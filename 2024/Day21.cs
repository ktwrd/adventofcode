using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(21)]
public class Day21 : IDayHandler
{
    public static ReadOnlyCollection<(Point, char)> Directions = new List<(Point, char)>()
    {
        (new(0, -1), '^'),
        (new(0, 1), 'v'),
        (new(1, 0), '>'),
        (new(-1, 0), '<')
    }.AsReadOnly();
    public static ReadOnlyCollection<char[]> Numberpad = new List<char[]>()
    {
        "#####".ToCharArray(),
        "#789#".ToCharArray(),
        "#456#".ToCharArray(),
        "#123#".ToCharArray(),
        "##0A#".ToCharArray(),
        "#####".ToCharArray(),
    }.AsReadOnly();
    public static ReadOnlyCollection<char[]> NumberpadDirections = new List<char[]>()
    {
        "#####".ToCharArray(),
        "##^A#".ToCharArray(),
        "#<v>#".ToCharArray(),
        "#####".ToCharArray(),
    }.AsReadOnly();
    public void Run(string[] inputData)
    {
        var inputDataChar = inputData.Select(e => e.ToCharArray()).ToArray();
        var a = Process(inputDataChar, 2);
        Console.WriteLine($"Part One: {a}");
        var b = Process(inputDataChar, 25);
        Console.WriteLine($"Part Two: {b}");
    }

    public long Process(char[][] inputData, long depth)
    {
        long r = 0;

        foreach (var p in inputData)
        {
            var s = long.Parse(new string(p).TrimEnd('A'));

            r += s * Execute(Numberpad.ToArray(), p, depth, []);
        }

        return r;
    }

    public string[] FindAllMinimumPaths(char[][] map, char startValue, char endValue)
    {
        int height = map.Length;
        int width = map[0].Length;
        var start = new Point(-1, -1);
        for (int y = 0; y < map.Length; y++)
        {
            var row = map[y];
            for (int x = 0; x < map[y].Length; x++)
            {
                var v = map[y][x];
                if (v == startValue)
                {
                    start = new(x, y);
                }
            }
        }

        var queue = new Queue<(Point pos, List<char> path, long cost, int? directionId)>();
        queue.Enqueue((start, [], 0, null));
        long minimumCost = long.MaxValue;
        Point? endPosition = null;
        var paths = new List<List<char>>();
        var seen = new Dictionary<string, long>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.directionId != null)
            {
                current.path.Add(Directions[current.directionId.Value].Item2);
            }

            if (map[current.pos.Y][current.pos.X] == endValue)
            {
                endPosition = current.pos;
                if (current.cost < minimumCost)
                {
                    paths = [];
                    minimumCost = current.cost;
                }

                if (current.cost == minimumCost)
                {
                    paths.Add(current.path);
                    continue;
                }
            }

            var k = $"{current.pos.X}_{current.pos.Y}";
            if (seen.ContainsKey(k))
            {
                if (seen[k] < current.cost)
                {
                    continue;
                }
            }

            seen[k] = current.cost;
            if (current.cost > minimumCost)
                continue;

            for (int i = 0; i < Directions.Count; i++)
            {
                var (dirOffset, _) = Directions[i];
                var newpos = new Point(current.pos.X + dirOffset.X, current.pos.Y + dirOffset.Y);
                if (newpos.X >= 0 && newpos.X < width && newpos.Y >= 0 && newpos.Y < height)
                {
                    if (map[newpos.Y][newpos.X] != '#')
                    {
                        queue.Enqueue((newpos, [..current.path], current.cost + 1, i));
                    }
                }
            }
        }

        return paths.Select(e => string.Join(string.Empty, e) + "A").ToArray();
    }
    public long Execute(char[][] map, char[] code, long depth, Dictionary<string, long> memo)
    {
        var k = new string(code) + $"_{depth}";
        if (memo.ContainsKey(k))
            return memo[k];

        var currentPosition = 'A';
        long length = 0;

        foreach (var c in code)
        {
            var paths = FindAllMinimumPaths(map, currentPosition, c);
            if (depth == 0)
            {
                length += paths[0].LongCount();
            }
            else
            {
                var d = paths.Select(
                    e => Execute(NumberpadDirections.ToArray(), e.ToCharArray(), depth - 1, memo));
                length += d.OrderBy(e => e).First();
            }

            currentPosition = c;
        }

        memo[k] = length;
        return length;
    }
}