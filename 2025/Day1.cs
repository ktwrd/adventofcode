namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 1)]
public class Day1 : IDayHandler
{
    public void Run(string[] data, out object partOne, out object partTwo)
    {
        int value = 50;
        int zero = 0;
        int zeroHits = 0;
        for (int i = 0; i < data.Length; i++)
        {
            var shiftValue = int.Parse(data[i].Substring(1));
            var delta = data[i][0] == 'L' ? -1 : 1;

            for (int q = 0; q < shiftValue; q++)
            {
                value += delta;
                if (value > 99)
                    value = 0;
                if (value < 0)
                    value = 99;
                if (value == 0) zeroHits++;
            }
            if (value == 0) zero++;
        }
        partOne = zero;
        partTwo = zeroHits;
    }
}
