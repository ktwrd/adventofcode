using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.TwentyTwentyFour;

public class DayOne
{
    public void Run(string[] lines)
    {
        var dataA = new List<int>();
        var dataB = new List<int>();
        foreach (var line in lines)
        {
            var split = line.Split(" ");
            if (split.Length < 2)
                continue;
            var a = split.First();
            var b = split.Last();
            dataA.Add(int.Parse(a));
            dataB.Add(int.Parse(b));
        }

        dataA = dataA.OrderBy(e => e).ToList();
        dataB = dataB.OrderBy(e => e).ToList();

        var data = new List<(int, int)>();
        for (int i = 0; i < dataA.Count; i++)
        {
            data.Add((dataA[i], dataB[i]));
        }

        PartA(data);
        PartB(dataA, dataB);
    }
    public void PartA(List<(int, int)> data)
    {
        int sum = 0;
        foreach (var (a, b) in data)
        {
            if (a > b)
            {
                sum += (a - b);
            }
            else
            {
                sum += (b - a);
            }
        }
        Console.WriteLine($"A: {sum}");
    }
    public void PartB(List<int> a, List<int> b)
    {
        int result = 0;
        foreach (var i in a)
        {
            int c = 0;
            foreach (var j in b)
            {
                if (j == i)
                    c++;
            }
            result += i * c;
        }
        Console.WriteLine($"B: {result}");
    }
}
