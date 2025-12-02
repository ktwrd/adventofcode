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
        var watchA = new Stopwatch();
        watchA.Start();
        var a = Part1(lines);
        watchA.Stop();
        Console.WriteLine($"Part 1: {a} ({watchA.ElapsedMilliseconds}ms)");
        var watchB = new Stopwatch();
        watchB.Start();
        var b = Part2(lines);
        watchB.Stop();
        Console.WriteLine($"Part 2: {b} ({watchB.ElapsedMilliseconds}ms)");
    }

    private int Part1(string[] input)
    {
        int count = 0;
        var inputWhole = string.Join("\n", input);
        foreach (var _ in Regex.Matches(inputWhole, @"(XMAS)", RegexOptions.None))
        {
            count++;
        }
        foreach (var _ in Regex.Matches(inputWhole, @"(SAMX)", RegexOptions.None))
        {
            count++;
        }

        var rowLength = input[0].Length;
        var p = rowLength - 3;
        for (int i = 0; i < input.Length - 3; i++)
        {
            for (int c = 0; c < rowLength; c++)
            {
                // vertical
                if (input[i][c] == 'X'
                    && input[i + 1][c] == 'M'
                    && input[i + 2][c] == 'A'
                    && input[i + 3][c] == 'S')
                {
                    count++;
                }
                if (input[i][c] == 'S'
                    && input[i + 1][c] == 'A'
                    && input[i + 2][c] == 'M'
                    && input[i + 3][c] == 'X')
                {
                    count++;
                }

                // diagonal - to left, downwards
                if (c >= 3)
                {
                    if (input[i][c] == 'X'
                        && input[i + 1][c - 1] == 'M'
                        && input[i + 2][c - 2] == 'A'
                        && input[i + 3][c - 3] == 'S')
                    {
                        count++;
                    }
                
                    if (input[i][c] == 'S'
                        && input[i + 1][c - 1] == 'A'
                        && input[i + 2][c - 2] == 'M'
                        && input[i + 3][c - 3] == 'X')
                    {
                        count++;
                    }
                }

                // diagonal - to right, downwards
                if (c < p)
                {
                    if (input[i][c] == 'X'
                        && input[i + 1][c + 1] == 'M'
                        && input[i + 2][c + 2] == 'A'
                        && input[i + 3][c + 3] == 'S')
                    {
                        count++;
                    }
                    if (input[i][c] == 'S'
                        && input[i + 1][c + 1] == 'A'
                        && input[i + 2][c + 2] == 'M'
                        && input[i + 3][c + 3] == 'X')
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private int Part2(string[] input)
    {
        var rowLength = input[0].Length;
        int count = 0;
        var xp = "MAS".ToCharArray();
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
                    // top right to bottom left (reversed)
                    bool d = input[i + 2][c] == s[2]
                             && input[i + 1][c + 1] == s[1]
                             && input[i][c + 2] == s[0];
                    // top left to bottom right (reversed)
                    bool e = input[i][c] == s[2]
                             && input[i + 1][c + 1] == s[1]
                             && input[i + 2][c + 2] == s[0];
                    var arr = new[]
                    {
                        a, b, d, e
                    };
                    return arr.Count(x => x) >= 2;
                }

                if (Check(xp))
                {
                    count++;
                }
            }
        }

        return count;
    }
}