using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(25)]
public class Day25 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var keys = new Dictionary<int, List<int[]>>();
        var locks = new Dictionary<int, List<int[]>>();

        for (int n = 0; n < 6; n++)
        {
            keys[n] = [];
            locks[n] = [];
        }

        string[] patterns = string.Join("\n", inputData)
            .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string pattern in patterns)
        {
            var rows = pattern.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var pins = new int[5];
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (rows[i][j] == '#')
                        pins[j]++;
                }
            }

            if (rows[0][0] == '#')
            {
                locks[pins[0]].Add(pins.Skip(1).ToArray());
            }
            else
            {
                keys[pins[0]].Add(pins.Skip(1).ToArray());
            }
        }

        int totalCount = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6 - i; j++)
            {
                totalCount += CountMatch(keys[i], locks[j]);
            }
        }

        Console.WriteLine($"Solution: {totalCount}");
    }
    private static int CountMatch(List<int[]> keys, List<int[]> locks)
    {
        var count = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                int maxPin = 0;
                for (int i = 0; i < 4; i++)
                {
                    maxPin = Math.Max(maxPin, key[i] + @lock[i]);
                }

                if (maxPin <= 5) count++;
            }
        }
        return count;
    }
}