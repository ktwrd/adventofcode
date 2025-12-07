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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AdventOfCode;

public class AdventHandlerBuilder
{
    private string _dataDirectory = "./data";
    private bool _allowReadlineForDay = true;
    private int? _year = null;
    private readonly List<Type> _types = [];

    public IReadOnlyCollection<Type> Types => _types;

    public AdventHandlerBuilder WithType(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        Type type)
    {
        _types.Add(type);
        return this;
    }
    public AdventHandlerBuilder WithType<T>()
        where T : class, IDayHandler, new()
    {
        return WithType(typeof(T));
    }

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
        handler.RegisterTypes(_types);
        var type = handler.RegisteredTypes
            .FirstOrDefault(e => e.Attribute.Year == _year && e.Attribute.Day == dayIdent)
            ?? throw new InvalidOperationException($"Could not find registered type for year {_year} and day {dayIdent}");
        handler.Execute(ref type);
    }

    private int FindDayIdentifier(params string[] args)
    {
        string inputValue = "";
        var now = DateTime.Now;
        if (args.Length == 0 && now.Month == 12)
        {
            Console.WriteLine($"It's December!!! Assuming today ({now.Day})");
            inputValue = now.Day.ToString();
        }
        else if (args.Length == 0 && _allowReadlineForDay)
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
}