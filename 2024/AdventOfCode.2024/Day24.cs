using System.ComponentModel;
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
        foreach (var k in b.Keys.Where(e => e.StartsWith("z")).OrderByDescending(e => e))
        {
            var ss = b[k] ? '1' : '0';
            s += ss;
            // Console.WriteLine($"{k}: {ss}");
        }
        Console.WriteLine(s);
        return Convert.ToInt64(s, 2);
    }

    private string PartTwo(string[] inputData)
    {
        _processedInstructions = [];
        var (a, b) = ParseInput(inputData);
        var swapped = new List<string>();
        foreach (var i in a)
        {
            var rx = new Regex("([a-z])([a-z0-9]{2})");
            var leftMatch = rx.Match(i.Left);
            if (leftMatch.Success)
            {
                var pipeToMatch = rx.Match(i.PipeTo);
                if (pipeToMatch.Groups.Count == leftMatch.Groups.Count)
                {
                    if (pipeToMatch.Groups[2].Value != leftMatch.Groups[2].Value)
                    {
                        swapped.Add(i.PipeTo);
                        i.PipeTo = pipeToMatch.Groups[1].Value + leftMatch.Groups[2].Value;
                    }
                }
            }
        }
        foreach (var i in a)
        {
            Solve(i, a, b);
        }

        var result = string.Join(",", swapped.OrderBy(e => e));
        return result;
    }

    private List<string> _processedInstructions = [];
    private void Solve(PathItem current, List<PathItem> instructions, Dictionary<string, bool> states)
    {
        if (_processedInstructions.Contains(current.ToString()))
            return;
        if (states.ContainsKey(current.Left) == false)
        {
            foreach (var i in instructions)
            {
                if (i.PipeTo == current.Left && i != current)
                {
                    Solve(i, instructions, states);
                    break;
                }
            }
        }

        if (states.ContainsKey(current.Right) == false)
        {
            foreach (var i in instructions)
            {
                if (i.PipeTo == current.Right && i != current)
                {
                    Solve(i, instructions, states);
                    break;
                }
            }
        }

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