using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(13)]
public class DayThirteen : IDayHandler
{
    public void Run(string[] lines)
    {
        var a = PartOne(lines);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(lines);
        Console.WriteLine($"Part Two: {b}");
    }

    public struct Goal
    {
        public Point A;
        public Point B;
        public Point Prize;
    }

    public List<Goal> ParseGoals(string[] lines)
    {
        var result = new List<Goal>();
        var a = Point.Empty;
        var b = Point.Empty;
        var prize = Point.Empty;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            void Append()
            {
                result.Add(new Goal()
                {
                    A = a,
                    B = b,
                    Prize = prize
                });
            }
            if (string.IsNullOrEmpty(line))
            {
                Append();
                continue;
            }

            if (line.StartsWith("Button A") || line.StartsWith("Button B"))
            {
                var split = line.Split(':')[1];
                var comma = split.Split(',');
                var strX = comma[0].Split('X')[1];
                strX = strX.Substring(strX.IndexOf('+') + 1);
                var strY = comma[1].Split('Y')[1];
                strY = strY.Substring(strY.IndexOf('+') + 1);

                var x = int.Parse(strX);
                var y = int.Parse(strY);
                if (line.StartsWith("Button A"))
                {
                    a = new(x, y);
                }
                else if (line.StartsWith("Button B"))
                {
                    b = new(x, y);
                }
            }
            else if (line.StartsWith("Prize"))
            {
                var split = line.Split(":")[1];
                var strX = split.Split("X=")[1].Split(',')[0];
                var strY = split.Split("Y=")[1].Split(',')[0];
                
                prize = new(int.Parse(strX), int.Parse(strY));
            }
            
            if (i == lines.Length - 1)
            {
                Append();
            }
        }
        return result;
    }
    private long CostToWin(Goal goal, long offset)
    {
        var ax = goal.A.X;
        var ay = goal.A.Y;
        var bx = goal.B.X;
        var by = goal.B.Y;
        var zx = goal.Prize.X + offset;
        var zy = goal.Prize.Y + offset;

        var pb = ((zx * ay) - (zy * ax)) / ((bx * ay) - (ax * by));
        var pa = (zx - (pb * bx))/ax;

        if ((pa * ax + pb * bx) == zx
            && pa * ay + pb * by == zy)
        {
            return pa * 3 + pb;
        }

        return 0;
    }
    public long PartOne(string[] lines)
    {
        long result = 0;
        
        foreach (var goal in ParseGoals(lines))
        {
            result += CostToWin(goal, 0);
        }
        return result;
    }

    public long PartTwo(string[] lines)
    {
        long result = 0;
        
        foreach (var goal in ParseGoals(lines))
        {
            result += CostToWin(goal, 10000000000000);
        }
        return result;
    }
}