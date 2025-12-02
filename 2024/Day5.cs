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
        
        var printOrder = EnsurePrintOrder(orderingRules, existingPageNumbers, out var partOneData, out var partTwoData);
        
        var partOne = FindMiddleSum(partOneData, printOrder);
        var partTwo = FindMiddleSum(partTwoData, printOrder);
        Console.WriteLine($"Part 1: {partOne}");
        Console.WriteLine($"Part 2: {partTwo}");
    }

    private List<List<int>> EnsurePrintOrder(
        (int, int)[] orderingRules,
        List<List<int>> currentPrintOrder,
        out List<int> unchangedIndexes,
        out List<int> shiftedIndexes)
    {
        var result = new List<List<int>>(currentPrintOrder.Count);

        unchangedIndexes = [];
        shiftedIndexes = [];
        for (int i = 0; i < currentPrintOrder.Count; i++)
        {
            var current = new List<int>(currentPrintOrder[i]);
            bool allowLoop = true;
            bool firstSuccess = false;
            int attemptCount = 0;
            while (allowLoop)
            {
                allowLoop = VerifyPrintOrder(current, orderingRules, out var failedAt) == false;
                if (attemptCount == 0 && allowLoop == false)
                {
                    firstSuccess = true;
                }

                if (failedAt != null)
                {
                    // shift failed item to the left. pretty much brute forcing
                    var fai = (int)failedAt;
                    var c = current[fai];
                    var cc = current[fai - 1];
                    current[fai] = cc;
                    current[fai - 1] = c;
                }

                attemptCount++;
            }

            if (firstSuccess)
            {
                unchangedIndexes.Add(i);
            }
            else
            {
                shiftedIndexes.Add(i);
            }
            
            result.Add(current);
        }

        return result;
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
        foreach (var t in lines)
        {
            if (s == false)
            {
                if (string.IsNullOrEmpty(t))
                {
                    s = true;
                    continue;
                }
            }
            
            if (s)
            {
                var split = t.Split(',');
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
            foreach (var (x, y) in orderingRules)
            {
                if (printOrder[i] == x)
                {
                    var yi = printOrder.IndexOf(y);
                    if (yi > -1 && yi < i)
                    {
                        failedAt = i;
                        return false;
                    }
                }
            }
        }

        return true;
    }
}