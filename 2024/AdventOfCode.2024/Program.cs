namespace AdventOfCode.TwentyTwentyFour;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine($"Enter day:");
            var line = Console.ReadLine();
            if (int.TryParse(line, out var i))
            {
                Load(i);
            }
            else
            {
                Console.WriteLine($"Unknown value \"{line}\"");
            }
        }
        else
        {
            if (int.TryParse(args[0], out var i))
            {
                Load(i);
            }
            else
            {
                Console.WriteLine($"Unknown value \"{args[0]}\"");
            }
        }
    }
    static void Load(int day)
    {
        var file = File.ReadAllLines($"data/{day}.txt");
        switch (day)
        {
            case 1:
                new DayOne().Run(file);
                return;
            default:
                throw new ArgumentException($"Invalid value {day}", nameof(day));
        }
    }
}
