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

using System.Reflection;
using AdventOfCode;
using AdventOfCode.TwentyTwentyFive;
using BenchmarkDotNet.Running;
using System.CommandLine;

var builder = new AdventHandlerBuilder()
    .WithYear(2025)
    .WithType(typeof(Day1))
    .WithType(typeof(Day2))
    .WithType(typeof(Day3))
    .WithType(typeof(Day4))
    .WithType(typeof(Day5))
    .WithType(typeof(Day6))
    .WithType(typeof(Day7))
    .WithType(typeof(Day8))
    .WithType(typeof(Day9));


if (args.Length == 0 && DateTime.Now.Month == 12 && DateTime.Now.Day < 26)
{
    builder.Run(args);
    Environment.Exit(0);
}

var commandBenchDay = new Argument<int?>("day")
{
    Description = "Specific day to benchmark."
};
var commandBench = new Command("bench", "Run one or more benchmarks.")
{
    commandBenchDay
};
commandBench.SetAction(result =>
{
    var dayValue = result.GetValue(commandBenchDay);
    if (dayValue.HasValue)
    {
        RunBenchSpecific(dayValue.Value);
    }
    else
    {
        RunBench();
    }
});

var commandRunDay = new Argument<int?>("day")
{
    Description = "Specific day to run."
};
var commandRun = new Command("run", "Run the AOC Solution. Input data is expected to be in \"./data/<day>.txt\"")
{
    commandRunDay
};
commandRun.SetAction(result =>
{
    var errReason = DateTime.Now.Month != 12
        ? "It's not December"
        : DateTime.Now.Day >= 26 ? "It's December, but it's after the 25th."
        : "It should execute today's challenge! Not sure how we got here.";
    var day = DateTime.Now.Month == 12 && DateTime.Now.Day < 26
        ? result.GetValue(commandRunDay).GetValueOrDefault(DateTime.Now.Day)
        : result.GetRequiredValue(commandRunDay) ?? throw new InvalidOperationException("Missing required argument \"day\" ("+errReason+")");
    if (day < 1) throw new InvalidOperationException($"Day must range from 1 to 31 (got: {day})");
    builder.Run([
        day.ToString()
    ]);
});

var commandList = new Command("list", "List available puzzles (year and day)");
commandList.SetAction(result =>
{
    var plural = builder.Types.Count != 1 && builder.Types.Count != -1 ? "" : "s";
    Console.WriteLine($"{builder.Types.Count*100:n0} available solution{plural}");
    Console.WriteLine(string.Join(Environment.NewLine,
        builder.Types
            .Select(e => new
            {
                Type = e,
                Attr = e.GetCustomAttribute<AdventAttribute>()
                        ?? throw new InvalidOperationException($"Type {e.Namespace}.{e.Name} is missing attribute {typeof(AdventAttribute)}")
            })
            .OrderBy(e => e.Attr.Year).ThenBy(e => e.Attr.Day)
            .Select(e => $"{e.Attr.Year,4} - Day: {e.Attr.Day,2}")));
});

var rootCommand = new RootCommand()
{
    commandRun,
    commandBench,
    commandList
}.Parse(args).Invoke();

void RunBench()
{
    BenchmarkSwitcher
        .FromTypes([..
            builder.Types.Select(e => typeof(AdventBenchmarkRunner<>).MakeGenericType(e))
        ])
        .Run();
}
void RunBenchSpecific(int day)
{
    BenchmarkSwitcher
        .FromTypes([..
            builder.Types
                .Where(e => e.GetCustomAttribute<AdventAttribute>()?.Day == day)
                .Select(e => typeof(AdventBenchmarkRunner<>).MakeGenericType(e))
        ])
        .Run();
}