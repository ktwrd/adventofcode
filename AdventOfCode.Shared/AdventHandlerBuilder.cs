/*
 * MIT License
 * 
 * Copyright (c) 2022-2025 Kate Ward <kate@dariox.club>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace AdventOfCode;

public class AdventHandlerBuilder
{
    private string _dataDirectory = "./data";
    private bool _allowReadlineForDay = true;
    private int? _year = null;

    public AdventHandlerBuilder WithDataDirectory(DirectoryInfo directory)
    {
        _dataDirectory = directory.FullName;
        return this;
    }
    public AdventHandlerBuilder WithDataDirectory(string directory)
    {
        _dataDirectory = directory;
        return this;
    }

    public AdventHandlerBuilder WithReadlineForDayIdentifier(bool value)
    {
        _allowReadlineForDay = value;
        return this;
    }

    public AdventHandlerBuilder WithYear(int year)
    {
        if (year <= 2000) throw new ArgumentException($"Value must be greater than 2000 (got: {year})", nameof(year));
        _year = year;
        return this;
    }

    public void Run(params string[] args)
    {
        if (!Directory.Exists(_dataDirectory))
        {
            Console.Error.WriteLine(string.Join("\n",
                "Missing data directory! Please put the input data in a .txt file that has the name of the day.",
                $"E.g; Day 1 data: {_dataDirectory}/1.txt",
                $"     Day 2 data: {_dataDirectory}/2.txt",
                "     etc..."));
            Environment.Exit(1);
            return;
        }
        var dayIdent = FindDayIdentifier(args);
        var handler = new AdventHandler(new DirectoryInfo("./data"));
        var type = handler.RegisteredTypes
            .FirstOrDefault(e => e.Attribute.Year == _year && e.Attribute.Day == dayIdent)
            ?? throw new InvalidOperationException($"Could not find registered type for year {_year} and day {dayIdent}");
        handler.Execute(ref type);
    }

    private int FindDayIdentifier(params string[] args)
    {
        string inputValue = "";
        if (args.Length == 0 && _allowReadlineForDay)
        {
            Console.WriteLine($"Enter day (1-31):");
            inputValue = Console.ReadLine() ?? "";
        }
        else if (args.Length > 0)
        {
            inputValue = args[0];
        }

        if (int.TryParse(inputValue, out var result))
            return result;

        throw new InvalidOperationException($"Could not parse value into integer: \"{inputValue}\"");
    }
    public static void RunHandler(int year, params string[] args)
    {
        if (!Directory.Exists("./data"))
        {
            Console.Error.WriteLine(string.Join("\n",
                "Missing data directory! Please put the input data in a .txt file that has the name of the day.",
                "E.g; Day 1 data: ./data/1.txt",
                "     Day 2 data: ./data/2.txt",
                "     etc..."));
            Environment.Exit(1);
            return;
        }
        int? dayIdent = null;
        if (args.Length == 0)
        {
            Console.WriteLine($"Enter day (1-31):");
            var value = Console.ReadLine();
            if (int.TryParse(value, out var dayIdentX))
            {
                dayIdent = dayIdentX;
            }
            else
            {
                Console.WriteLine($"Could not parse \"{value}\" into an integer");
            }
        }
        else
        {
            if (int.TryParse(args[0], out var dayIdentX))
            {
                dayIdent = dayIdentX;
            }
            else
            {
                Console.WriteLine($"Could not parse \"{args[0]}\" into an integer");
            }
        }
        if (dayIdent == null)
        {
            Environment.Exit(1);
        }

        var handler = new AdventHandler(new DirectoryInfo("./data"));
        var type = handler.RegisteredTypes.FirstOrDefault(e => e.Attribute.Year == year && e.Attribute.Day == dayIdent.Value)
            ?? throw new InvalidOperationException($"Could not find registered type for year {year} and day {dayIdent}");
        handler.Execute(ref type);
    }
}