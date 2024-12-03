using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(3)]
public class DayThree : IDayHandler
{
    public void Run(string[] lines)
    {
        var content = string.Join("\n", lines);
        var a = PartA(content);
        Console.WriteLine($"Part One: {a}");
        var b = PartB(content);
        Console.WriteLine($"Part Two: {b}");
    }

    private int PartA(string content, Func<Match, bool>? filter = null)
    {
        filter ??= (_) => true;
        var rx = new Regex(@"(mul\([0-9]{1,3}\,[0-9]{1,3}\))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        var inner = new Regex(@"mul\(([0-9]{1,3})\,([0-9]{1,3})\)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        int result = 0;
        foreach (Match match in rx.Matches(content))
        {
            var innerMatch = inner.Match(match.Value);
            var a = innerMatch.Groups[1].Value;
            var b = innerMatch.Groups[2].Value;
            if (filter(match))
            {
                result += int.Parse(a) * int.Parse(b);
            }
        }

        return result;
    }

    private int PartB(string content)
    {
        var doIndexes = new List<int>();
        var dontIndexes = new List<int>();
        var doRegex = new Regex(@"(do\(\))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        foreach (Match match in doRegex.Matches(content))
        {
            doIndexes.Add(match.Index);
        }
        var dontRegex = new Regex(@"(don't\(\))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        foreach (Match match in dontRegex.Matches(content))
        {
            dontIndexes.Add(match.Index);
        }

        List<(int, int)> FindClosest(List<int> indexes, int matchIndex)
        {
            return indexes.Select(e => (e, matchIndex - e))
                .Where(e => e.Item2 > 0)
                .OrderBy(e => e.Item2)
                .ToList();
        }

        bool FilterAction(Match inner)
        {
            int allowIndex = -1;
            int disallowIndex = -1;

            var allowSorted = FindClosest(doIndexes, inner.Index);
            var disallowSorted = FindClosest(dontIndexes, inner.Index);
            if (allowSorted.Count > 0)
            {
                allowIndex = allowSorted[0].Item1;
            }

            if (disallowSorted.Count > 0)
            {
                disallowIndex = disallowSorted[0].Item1;
            }

            if (allowIndex != -1 && disallowIndex == -1)
                return true;
            if (allowIndex == -1 && disallowIndex == -1)
                return true;
            if (allowIndex == -1 && disallowIndex != -1)
                return false;

            return allowIndex > disallowIndex;
        }

        var result = PartA(content, FilterAction);
        

        return result;
    }
}