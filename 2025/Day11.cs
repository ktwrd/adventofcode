namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 11)]
public class Day11 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        var racks = content.Select(e => new ServerRack(e)).ToArray();
        var p2r = racks.Concat([new("out: ")]).ToArray();
        partOne = SolvePartOne(ref racks);
        partTwo = SolvePartTwo(ref p2r);
        // partTwo = Solve(false, ref racks);
    }

    private int SolvePartOne(
        ref ServerRack[] racks)
    {
        var pathsVisited = new HashSet<string>();
        var queue = new Queue<(string path, ServerRack rack)>();
        queue.Enqueue(("", racks.Single(e => e.Name == "you")));
        var total = 0;
        while (queue.Count > 0)
        {
            var (path, rack) = queue.Dequeue();
            var sp = path + rack.Name + ",";
            if (rack.Outputs.Any(e => e == "out"))
            {
                total++;
                continue;
            }

            foreach (var r in racks.Where(e => rack.Outputs.Contains(e.Name)))
            {
                queue.Enqueue((sp, r));
            }
        }
        return total;
    }

    private Dictionary<string, long> _p2Dict = [];

    private long SolvePartTwo(
        ref ServerRack[] racks)
    {
        _p2Dict = [];
        return SolvePartTwo(ref racks, "svr");
    }
    private long SolvePartTwo(ref ServerRack[] racks,
    string name, bool a = false, bool b = false)
    {
        var h = $"{name},{a},{b}";
        if (_p2Dict.ContainsKey(h)) return _p2Dict[h];

        if (name == "out")
        {
            if (a && b)
            {
                return 1;
            }
            return 0;
        }

        var rack = racks.Single(e => e.Name == name);

        long t = 0;
        foreach (var output in rack.Outputs)
        {
            t += SolvePartTwo(ref racks, output,
                rack.Name == "fft" || a, rack.Name == "dac" || b);
        }
        _p2Dict[h] = t;
        return t;
    }

    public record ServerRack
    {
        public ServerRack(string line)
        {
            var i = line.IndexOf(':');
            Name = line[..i];
            Outputs = line[(i + 2)..].Split(' ');
        }
        public readonly string Name;
        public readonly string[] Outputs;
    }
}
