using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(24)]
public class Day24 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    public (List<PathItem> instructions, Dictionary<string, bool> states) ParseInput(string[] inputData)
    {
        var instructions = new List<PathItem>();
        var wires = new Dictionary<string, bool>();
        bool parsePaths = false;
        foreach (var line in inputData)
        {
            if (string.IsNullOrEmpty(line))
            {
                parsePaths = true;
                continue;
            }


            var split = line.Split(' ');
            if (parsePaths)
            {
                var item = new PathItem()
                {
                    Left = split[0],
                    Right = split[2],
                    Operation = PathOperation.Unknown,
                    PipeTo = split[4]
                };
                if (split[1] == "AND")
                {
                    item.Operation = PathOperation.AND;
                }
                else if (split[1] == "OR")
                {
                    item.Operation = PathOperation.OR;
                }
                else if (split[1] == "XOR")
                {
                    item.Operation = PathOperation.XOR;
                }
                instructions.Add(item);
            }
            else
            {
                wires[split[0].Split(":")[0].Trim()] = split[1] == "1";
            }
        }
        return (instructions, wires);
    }

    public class PathItem
    {
        public string Left;
        public string Right;
        public PathOperation Operation;
        public string PipeTo;
        public bool IsDirect => Left.IndexOf('x') == 0 || Right.IndexOf('x') == 0;

        public bool ConsiderResult => PipeTo.IndexOf('z') == 0;

        public override string ToString()
        {
            return $"{Left} {Operation} {Right} -> {PipeTo}";
        }
    }

    public enum PathOperation
    {
        Unknown = -1,
        AND,
        OR,
        XOR
    }
    private long PartOne(string[] inputData)
    {
        var (a, b) = ParseInput(inputData);
        foreach (var i in a)
        {
            Solve(i, a, b);
        }
        var s = "";
        var bitKeys = b.Keys.Where(e => e.StartsWith("z")).OrderBy(e => e).ToList();
        foreach (var k in bitKeys)
        {
            var ss = b[k] ? '1' : '0';
            s = ss + s;
        }
        return Convert.ToInt64(s, 2);
    }

    private string PartTwo(string[] inputData)
    {
        var (a, b) = ParseInput(inputData);
        var inputBitCount = b.Keys.Count / 2;
        var lastOutputPipe = "z" + inputBitCount.ToString().PadLeft(2, '0');
        var flags = new List<string>();
        var directXorItems = a.Where(e => e.IsDirect).Where(e => e.Operation == PathOperation.XOR).ToList();
        foreach (var item in directXorItems)
        {
            if (item.Left == "x00" || item.Right == "x00")
            {
                if (item.PipeTo != "z00")
                {
                    flags.Add(item.PipeTo);
                }

                continue;
            }
            else if (item.PipeTo == "z00")
            {
                flags.Add(item.PipeTo);
            }

            if (item.ConsiderResult)
            {
                flags.Add(item.PipeTo);
            }
        }

        var indirectXORItems = a.Where(e => e.IsDirect == false).Where(e => e.Operation == PathOperation.XOR).ToList();
        foreach (var item in indirectXORItems)
        {
            if (!item.ConsiderResult)
            {
                flags.Add(item.PipeTo);
            }
        }

        foreach (var item in a.Where(e => e.ConsiderResult))
        {
            if (item.PipeTo == lastOutputPipe)
            {
                if (item.Operation != PathOperation.OR)
                {
                    flags.Add(item.PipeTo);
                }
            }
            else if (item.Operation != PathOperation.XOR)
            {
                flags.Add(item.PipeTo);
            }
        }

        var nextCheckIndexes = new List<int>();
        for (int i = 0; i < directXorItems.Count; i++)
        {
            var item = directXorItems[i];

            if (flags.Contains(item.PipeTo))
                continue;
            if (item.PipeTo == "z00")
                continue;

            var matches = indirectXORItems.Where(e => e.Left == item.PipeTo || e.Right == item.PipeTo).ToList();
            if (matches.Count == 0)
            {
                nextCheckIndexes.Add(i);
                flags.Add(item.PipeTo);
            }
        }

        foreach (var item in nextCheckIndexes.Select(e => directXorItems[e]))
        {
            var targetPipeTo = "z" + item.Left.Substring(1);
            var matches = indirectXORItems.Where(e => e.PipeTo == targetPipeTo).ToList();
            if (matches.Count != 1)
            {
                throw new ApplicationException($"Input might be incorrect :/");
            }
            var targetMatch = matches[0];
            var matchesOR = a.Where(e => e.Operation == PathOperation.OR)
                .Where(e => e.PipeTo == targetMatch.Left || e.PipeTo == targetMatch.Right).ToList();
            if (matchesOR.Count != 1)
            {
                throw new ApplicationException($"Input might be incorrect :/ (failed to find OR operations that have an output of {targetMatch.Left} or {targetMatch.Right})");
            }

            var correct = "";
            if (targetMatch.Left != matchesOR[0].PipeTo)
            {
                correct = targetMatch.Left;
            }
            else if (targetMatch.Right != matchesOR[0].PipeTo)
            {
                correct = targetMatch.Right;
            }
            if (!string.IsNullOrEmpty(correct))
            {
                flags.Add(correct);
            }
        }

        if (flags.Count != 8)
        {
            Console.WriteLine($"Output might be invalid, only contains 8 items");
        }

        return string.Join(",", flags.OrderBy(e => e));
    }

    private List<string> _processedInstructions = [];

    private void EnsureParents(PathItem current, List<PathItem> instructions, Dictionary<string, bool> states)
    {
        EnsureParent(current.Left, instructions, states, (i) => i != current);
        EnsureParent(current.Right, instructions, states, (i) => i != current);
    }

    private void EnsureParent(string item, List<PathItem> instructions, Dictionary<string, bool> states, Func<PathItem, bool>? filter = null)
    {
        filter ??= (_) => true;
        if (states.ContainsKey(item) == false)
        {
            foreach (var i in instructions)
            {
                if (i.PipeTo == item)
                {
                    if (filter(i))
                    {
                        Solve(i, instructions, states);
                    }
                }
            }
        }
    }
    private void Solve(PathItem current, List<PathItem> instructions, Dictionary<string, bool> states)
    {
        if (_processedInstructions.Contains(current.ToString()))
            return;
        EnsureParents(current, instructions, states);
        switch (current.Operation)
        {
            case PathOperation.AND:
                states[current.PipeTo] = states[current.Left] && states[current.Right];
                break;
            case PathOperation.OR:
                states[current.PipeTo] = states[current.Left] || states[current.Right];
                break;
            case PathOperation.XOR:
                states[current.PipeTo] = states[current.Left] != states[current.Right];
                break;
        }
        _processedInstructions.Add(current.ToString());
    }
}