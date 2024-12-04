using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(4)]
public class DayFour : IDayHandler
{
    public void Run(string[] lines)
    {
        lines = lines.Where(e => !string.IsNullOrEmpty(e)).ToArray();
        var a = Part1(lines);
        Console.WriteLine($"Part 1: {a}");
        var b = Part2(lines);
        Console.WriteLine($"Part 2: {b}");
    }

    private int Part1(string[] input)
    {
        int count = 0;
        var inputWhole = string.Join("\n", input);
        int horizontal = 0;
        foreach (Match match in Regex.Matches(inputWhole, @"(XMAS)", RegexOptions.None))
        {
            Trace.WriteLine($"XMAS: {match.Index}");
            count++;
            horizontal++;
        }

        foreach (Match match in Regex.Matches(inputWhole, @"(SAMX)", RegexOptions.None))
        {
            Trace.WriteLine($"SAMX: {match.Index}");
            count++;
            horizontal++;
        }
        Trace.WriteLine($"Horizontal: {horizontal}");

        var rowLength = input[0].Length;
        for (int i = 0; i < input.Length - 3; i++)
        {
            // vertical
            for (int c = 0; c < rowLength; c++)
            {
                if (input[i][c] == 'X'
                    && input[i + 1][c] == 'M'
                    && input[i + 2][c] == 'A'
                    && input[i + 3][c] == 'S')
                {
                    count++;
                    Trace.WriteLine($"XMAS (Vertical) Line {i}, Char: {c}");
                }
                if (input[i][c] == 'S'
                         && input[i + 1][c] == 'A'
                         && input[i + 2][c] == 'M'
                         && input[i + 3][c] == 'X')
                {
                    count++;
                    Trace.WriteLine($"SAMX (Vertical) Line {i}, Char: {c}");
                }
            }
            // diagonal - to right, downwards
            for (int c = 0; c < rowLength - 3; c++)
            {
                if (input[i][c] == 'X'
                    && input[i + 1][c + 1] == 'M'
                    && input[i + 2][c + 2] == 'A'
                    && input[i + 3][c + 3] == 'S')
                {
                    count++;
                    Trace.WriteLine($"XMAS (Diagonal, Right) Line {i}, Char: {c}");
                }
                if (input[i][c] == 'S'
                         && input[i + 1][c + 1] == 'A'
                         && input[i + 2][c + 2] == 'M'
                         && input[i + 3][c + 3] == 'X')
                {
                    count++;
                    Trace.WriteLine($"SAMX (Diagonal, Right) Line {i}, Char: {c}");
                }
            }
            // diagonal - to left, downwards
            for (int c = 3; c < rowLength; c++)
            {
                if (input[i][c] == 'X'
                    && input[i + 1][c - 1] == 'M'
                    && input[i + 2][c - 2] == 'A'
                    && input[i + 3][c - 3] == 'S')
                {
                    Trace.WriteLine($"XMAS (Diagonal, Left) Line {i}, Char: {c}");
                    count++;
                }
                
                if (input[i][c] == 'S'
                    && input[i + 1][c - 1] == 'A'
                    && input[i + 2][c - 2] == 'M'
                    && input[i + 3][c - 3] == 'X')
                {
                    Trace.WriteLine($"SAMX (Diagonal, Left) Line {i}, Char: {c}");
                    count++;
                }
            }
        }
        return count;
    }

    private int Part2(string[] input)
    {
        var rowLength = input[0].Length;
        int count = 0;
        for (int i = 0; i < input.Length - 2; i++)
        {
            for (int c = 0; c < rowLength - 2; c++)
            {
                bool Check(char[] s)
                {
                    // top left to bottom right
                    bool a    = input[i][c] == s[0]
                                 && input[i + 1][c + 1] == s[1]
                                 && input[i + 2][c + 2] == s[2];
                    // top right to bottom left
                    bool b = input[i + 2][c] == s[0]
                                 && input[i + 1][c + 1] == s[1]
                                 && input[i][c + 2] == s[2];
                    bool d = input[i + 2][c] == s[2]
                             && input[i + 1][c + 1] == s[1]
                             && input[i][c + 2] == s[0];
                    bool e = input[i][c] == s[2]
                             && input[i + 1][c + 1] == s[1]
                             && input[i + 2][c + 2] == s[0];
                    var arr = new[]
                    {
                        a, b, d, e
                    };
                    return arr.Count(x => x) >= 2;
                }

                var xp = "MAS".ToCharArray();
                if (Check(xp))
                {
                    count++;
                }
            }
        }

        return count;
    }
}