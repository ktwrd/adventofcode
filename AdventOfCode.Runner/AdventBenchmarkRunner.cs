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
using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Runner;

/// <remarks>
/// god. i hate that having this in the target assembly is the easiest way to
/// use benchmarkdotnet without rewriting everything.
/// </remarks>
[MemoryDiagnoser(false)]
public class AdventBenchmarkRunner<TAdvent>
    where TAdvent : IDayHandler, new()
{
    private readonly TAdvent _puzzle = new();
    private string[] _rawInput = [];

    [GlobalSetup]
    public void Setup()
    {
        var attribute = typeof(TAdvent).GetCustomAttribute<AdventAttribute>()!;
        var t = new AdventHandler.AdventRegisteredType()
        {
            Type = typeof(TAdvent),
            Attribute = attribute ?? throw new InvalidOperationException($"Could not find AdventAttribute on {typeof(TAdvent)}")
        };
        _rawInput = AdventHandler.GetData(ref t);
    }

    [Benchmark]
    public (string, string) Solve()
    {
        _puzzle.Run(_rawInput, out var p1, out var p2);
        return (p1.ToString()??"", p2.ToString()??"");
    }
}
[MemoryDiagnoser(false)]
public class AdventBenchmarkRunnerGeneric
{
    private IDayHandler? _puzzle;
    private string[] _rawInput = [];

    [ParamsSource(nameof(YearValues))]
    public int Year { get; set; }

    [ParamsSource(nameof(DayValues))]
    public int Day { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var type = Program.GetTypes(Year).FirstOrDefault(e =>
        {
            var a = e.GetCustomAttribute<AdventAttribute>();
            return a?.Year == Year && a?.Day == Day;
        }) ?? throw new InvalidOperationException($"Could not find type for Year {Year} and Day {Day}");

        var instance = Activator.CreateInstance(type);
        if (instance is not IDayHandler handler)
        {
            throw new InvalidOperationException($"Type {type} does not implement {typeof(IDayHandler)}");
        }
        _puzzle = handler;
        var t = new AdventHandler.AdventRegisteredType()
        {
            Type = type,
            Attribute = type.GetCustomAttribute<AdventAttribute>() ?? throw new InvalidOperationException($"Could not find AdventAttribute on {type}")
        };
        _rawInput = AdventHandler.GetData(ref t);
    }
    
    [Benchmark]
    public (string, string) Solve()
    {
        if (_puzzle == null)
        {
            throw new InvalidOperationException($"{nameof(_puzzle)} was not set for Year {Year} Day {Day}");
        }

        _puzzle.Run(_rawInput, out var p1, out var p2);
        return (p1.ToString() ?? "", p2.ToString() ?? "");
    }

    public static IEnumerable<int> YearValues { get; set; } = [];
    public static IEnumerable<int> DayValues { get; set; } = [];
    public static ICollection<Type> GlobalTypes { get; set; } = [];
    public static void SetupGlobal(ICollection<Type> types)
    {
        GlobalTypes = types;
        YearValues = types.Select(e => e.GetCustomAttribute<AdventAttribute>()?.Year ?? 0).Where(e => e > 0).Distinct().ToList();
        DayValues = types.Select(e => e.GetCustomAttribute<AdventAttribute>()?.Day ?? 0).Where(e => e > 0).Distinct().ToList();
    }
}