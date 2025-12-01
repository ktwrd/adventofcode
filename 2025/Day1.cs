using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFive;

[DefaultValue(1)]
public class DayOne : IDayHandler
{
    public void Run(string[] data)
    {
        int value = 50;
        int zero = 0;
        int zeroHits = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (string.IsNullOrEmpty(data[i].Trim())) continue;
            data[i] = data[i].Trim();

            var shiftValue = int.Parse(data[i].Substring(1));
            var previousValue = int.Parse(value.ToString());
            
            var direction = data[i].ToCharArray()[0];
            var delta = direction == 'L' ? -1 : 1;

            for (int q = 0; q < shiftValue; q++)
            {
                value += delta;
                if (value > 99)
                    value = 0;
                if (value < 0)
                    value = 99;
                if (value == 0) zeroHits++;
            }
            if (value == 0)
            {
                zero++;
            }
            Console.WriteLine($"{data[i]}  {previousValue} -> {value}");
        }
        Console.WriteLine($"Current Value: {Math.Abs(value)}");
        Console.WriteLine($"Part A: {zero}");
        Console.WriteLine($"Part B: {zeroHits}");
    }
}
