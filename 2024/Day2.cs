using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(2)]
public class DayTwo : IDayHandler
{
    public void Run(string[] lines)
    {
        int result = 0;
        int secondResult = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line))
                continue;

            var data = line.Split(' ').Select(int.Parse).ToArray();

            if (IsSafe(data))
            {
                result++;
            }

            if (IsSafe(data))
            {
                secondResult++;
            }
            else
            {
                for (int x = 0; x < data.Length; x++)
                {
                    var tmp = line.Split(' ').Select(int.Parse).ToList();
                    tmp.RemoveAt(x);
                    if (IsSafe(tmp.ToArray()))
                    {
                        secondResult++;
                        break;
                    }
                }
            }
        }
        Console.WriteLine($"Part One: {result}");
        Console.WriteLine($"Part Two: {secondResult}");
    }

    public bool IsSafe(int[] data)
    {
        var a = Rule1(data);
        var b = Rule2(data);
        return a == false && b == false;
    }

    public bool Rule1(int[] data)
    {
        var p = new int[data.Length - 1];
        for (int i = 0; i < data.Length; i++)
        {
            if (i > 0)
            {
                p[i - 1] = data[i - 1] == data[i] ? 0 : data[i - 1] > data[i] ? 1 : -1;
            }
        }

        var a = 0;
        var b = 0;
        var c = 0;
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i] == 0)
                a++;
            else if (p[i] == 1)
                b++;
            else if (p[i] == -1)
                c++;
        }

        var s = true;
        if (a > 0 && b == 0 && c == 0)
            s = false;
        else if (a == 0 && b > 0 && c == 0)
            s = false;
        else if (a == 0 && b == 0 && c > 0)
            s = false;

        return s;
    }

    public bool Rule2(int[] data)
    {
        bool s = false;
        for (int i = 0; i < data.Length; i++)
        {
            if (i > 0)
            {
                var abs = Math.Abs(data[i] - data[i - 1]);
                if (abs > 3 || abs < 1)
                {
                    s = true;
                }
            }
        }

        return s;
    }
}