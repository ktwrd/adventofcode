using System.ComponentModel;
using System.Diagnostics;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(5)]
public class DayFive: IDayHandler
{
    public void Run(string[] lines)
    {
        var orderingRules = FindOrderingRules(lines);
        var existingPageNumbers = FindPageNumbers(lines);

        var firstSuccessIndexes = new List<int>();
        var notFirstSuccessfulIndexes = new List<int>();
        for (int i = 0; i < existingPageNumbers.Count; i++)
        {
            var current = new List<int>(existingPageNumbers[i]);
            Console.WriteLine("".PadRight(16, '=') + $" {i}");
            bool cont = true;
            bool firstSuccess = false;
            int attemptCount = 0;
            while (cont)
            {
                cont = VerifyPrintOrder(current, orderingRules, out var failedAt) == false;
                if (attemptCount == 0 && cont == false)
                {
                    firstSuccess = true;
                }

                if (failedAt != null)
                {
                    var fai = (int)failedAt;
                    var c = current[fai];
                    var cc = current[fai - 1];
                    current[fai] = cc;
                    current[fai - 1] = c;
                    Console.WriteLine($"Moved value {c} from index {fai} to {fai - 1}");
                }

                attemptCount++;
            }

            if (firstSuccess)
            {
                firstSuccessIndexes.Add(i);
            }
            else
            {
                notFirstSuccessfulIndexes.Add(i);
            }

            existingPageNumbers[i] = current;
            Console.WriteLine($"Finished Item");
        }
        Console.WriteLine($"Completed all lines");
        /*Console.WriteLine("======== All");
        foreach (var p in existingPageNumbers)
        {
            Console.WriteLine(string.Join(", ", p));
        }*/
        
        var partOne = FindMiddleSum(firstSuccessIndexes, existingPageNumbers);
        Console.WriteLine($"Part 1: {partOne}");
        var partTwo = FindMiddleSum(notFirstSuccessfulIndexes, existingPageNumbers);
        Console.WriteLine($"Part 2: {partTwo}");
    }

    private int FindMiddleSum(List<int> indexes, List<List<int>> existingPageNumbers)
    {
        int sum = 0;
        for (int i = 0; i < indexes.Count; i++)
        {
            var c = indexes[i];
            var val = existingPageNumbers[c];
            var midIndex = Convert.ToInt32(Math.Max(Math.Floor(val.Count / 2f), 0));
            var midVal = val[midIndex];
            sum += midVal;
        }

        return sum;
    }

    private (int, int)[] FindOrderingRules(string[] lines)
    {
        var res = new List<(int, int)>();
        foreach (var l in lines)
        {
            if (string.IsNullOrEmpty(l))
                break;
            var split = l.Split('|');
            if (split.Length == 2)
            {
                var a = int.Parse(split[0]);
                var b = int.Parse(split[1]);
                res.Add((a,b));
            }
        }

        return res.ToArray();
    }

    private List<List<int>> FindPageNumbers(string[] lines)
    {
        bool s = false;
        var result = new List<List<int>>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (s == false)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    s = true;
                    continue;
                }
            }
            
            if (s)
            {
                var split = lines[i].Split(',');
                var c = new List<int>(split.Length);
                c.AddRange(split.Select(int.Parse));
                result.Add(c);
            }
        }

        return result;
    }

    private bool VerifyPrintOrder(List<int> printOrder, (int, int)[] orderingRules, out int? failedAt)
    {
        failedAt = null;
        for (int i = 0; i < printOrder.Count; i++)
        {
            for (int c = 0; c < orderingRules.Length; c++)
            {
                var (x, y) = orderingRules[c];
                if (printOrder[i] == x)
                {
                    var yi = printOrder.IndexOf(y);
                    if (yi > -1 && yi < i)
                    {
                        failedAt = i;
                        Console.WriteLine($"{printOrder[i]} is before {y} (i: {i}, yi: {yi}, rule: {c})");
                        return false;
                    }
                }
            }
        }

        return true;
    }
}