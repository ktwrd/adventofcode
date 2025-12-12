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

using BenchmarkDotNet.Running;
using CSharpFunctionalExtensions;
using System.CommandLine;
using System.Reflection;

namespace AdventOfCode.Runner;

public static class Program
{
    public static void Main(string[] args)
    {
        var now = DateTime.Now;
        if (args.Length == 0 && now.Month == 12 && now.Day < 26)
        {
            Console.WriteLine("AOC Challenge could be executed!");
            ExecuteChallenge(now.Year, now.Day);
            return;
        }
        new RootCommand()
        {
            GetRunCommand(),
            GetBenchCommand(),
            GetListCommand()
        }.Parse(args).Invoke();
    }

    private static void ExecuteChallenge(int year, int day)
    {
        var builder = new AdventHandlerBuilder().WithYear(year);
        foreach (var type in GetTypes(year))
        {
            builder.WithType(type);
        }
        builder.Run([day.ToString()]);
    }

    private static readonly Argument<int> CommandRunYear = new("year")
    {
        Description = "AOC Year. Will default to this year when it's December.\n"+
                      "Required when it's not December.",
        DefaultValueFactory = _ => DateTime.Now.Month == 12 ? DateTime.Now.Year : -1
    };
    private static readonly Argument<int> CommandRunDay = new("day")
    {
        Description = "AOC Challenge Day. Will default to today when it's December, and before the 26th.\n"+
                      "Required when it's not December, or it's after 25th December.",
        DefaultValueFactory = _ => DateTime.Now.Month == 12 && DateTime.Now.Day < 26 ? DateTime.Now.Day : -1
    };
    private static Command GetRunCommand()
    {
        var command = new Command("run", "Run an AOC Solution. Input data is expected to be in \"./data/<year>/<day>.txt\"")
        {
            CommandRunDay,
            CommandRunYear
        };
        command.SetAction(result =>
        {
            var now = DateTime.Now;

            var yearValue = result.GetValue(CommandRunYear);
            var dayValue = result.GetValue(CommandRunDay);

            Result<int, string> year = yearValue > -1
                ? result.GetRequiredValue(CommandRunYear) < 2024
                    ? Result.Failure<int, string>("Year must be >= 2024")
                    : Result.Success<int, string>(result.GetRequiredValue(CommandRunYear))
                : now.Month == 12
                    ? Result.Success<int, string>(now.Year)
                    : "Cannot assume year since it's not December.";
            Result<int, string> day;
            if (dayValue > -1)
            {
                var requiredDay = result.GetRequiredValue(CommandRunDay);
                day = requiredDay < 1
                    ? "Day must be >= 1"
                    : requiredDay <= 25 ? requiredDay : "Day must be <= 25";
            }
            else
            {
                day = now.Month != 12
                    ? "Must provide day since it's not December."
                    : now.Day < 26 ? now.Day : "Cannot assume day, since it's after the 25th.";
            }

            if (year.IsFailure)
            {
                Console.Error.WriteLine("Cannot parse Year: " + year.Error);
            }
            if (day.IsFailure)
            {
                Console.Error.WriteLine("Cannot parse Day: " + year.Error);
            }
            if (year.IsSuccess && day.IsSuccess)
            {
                ExecuteChallenge(year.Value, day.Value);
            }
        });
        return command;
    }

    private static readonly Option<int> CommandBenchYear = new("--year")
    {
        Description = "AOC Year Filter. Will default to this year when it's December.\n"+
                      "Required when it's not December, or it's after December 25th.",
        Required = false
    };
    private static readonly Option<List<string>> CommandBenchArgs = new("--args")
    {
        Description = "Arguments for BenchmarkDotNet",
        Required = false,
        DefaultValueFactory = _ => [],
        AllowMultipleArgumentsPerToken = true
    };
    private static readonly Option<bool> CommandBenchYearParallel = new("--parallel")
    {
        Description = "Run all solutions for the \"--year\" value.",
        Required = false,
    };

    private static Command GetBenchCommand()
    {
        var command = new Command("bench")
        {
            CommandBenchYear,
            CommandBenchArgs,
            CommandBenchYearParallel
        };
        command.SetAction(result =>
        {
            var now = DateTime.Now;
            var yearValue = result.GetValue(CommandRunYear);
            Result<int, string> year = yearValue > 0
                ? yearValue < 2024
                    ? Result.Failure<int, string>("Year must be >= 2024")
                    : Result.Success<int, string>(yearValue)
                : now.Month == 12 && now.Day < 26
                    ? Result.Success<int, string>(now.Year)
                    : "Cannot assume year since it's not December (or it's after December 25th)";
            var extraArgs = result.GetValue(CommandBenchArgs);
            if (year.IsFailure)
            {
                Console.Error.WriteLine("Cannot parse Year: " + year.Error);
            }
            else
            {
                if (now.Month == 12)
                {
                    Console.WriteLine("[BenchCommand] Assumed year: " + now.Year);
                }
                var types = GetTypes(year.Value);
                if (result.GetValue(CommandBenchYearParallel))
                {
                    types.Remove(typeof(TwentyTwentyFive.Day10)); // ignore 2025/10 since it's so fucking slow
                    AdventBenchmarkRunnerGeneric.SetupGlobal(types);
                    BenchmarkSwitcher.FromTypes([typeof(AdventBenchmarkRunnerGeneric)]).RunAllJoined();
                }
                else
                {
                    BenchmarkSwitcher
                        .FromTypes([..
                            types.Select(e => typeof(AdventBenchmarkRunner<>).MakeGenericType(e))
                        ]).Run(extraArgs?.ToArray());
                }
            }
        });
        return command;
    }

    private static Command GetListCommand()
    {
        var command = new Command("list");
        command.SetAction(result =>
        {
            var sortedTypes = GetTypes()
                .Select(e => new
                {
                    Type = e,
                    Attr = e.GetCustomAttribute<AdventAttribute>()
                        ?? throw new InvalidOperationException($"Missing {typeof(AdventAttribute)} on type {e}")
                })
                .OrderBy(e => e.Attr.Year).ThenBy(e => e.Attr.Day)
                .ToList();

            var yearItemCount = string.Join(" | ", sortedTypes.GroupBy(e => e.Attr.Year).Select(e => $"{e.Key}={e.Count():n0}"));
            var plural = Math.Abs(sortedTypes.Count) == 1 ? "" : "s";
            var head = $"-- {sortedTypes.Count} available solution{plural} ({yearItemCount})";

            Console.WriteLine(head);
            Console.WriteLine(string.Join(Environment.NewLine,
                sortedTypes.Select(e => $"{e.Attr.Year} - {e.Attr.Day}")));
            Console.WriteLine(head);
        });
        return command;
    }
    public static ICollection<Type> GetTypes(int? yearFilter = null)
    {
        var result = new List<Type>();
        if (yearFilter == null || yearFilter == 2025)
        {
            result.Add2025Types();
        }
        return result;
    }
}