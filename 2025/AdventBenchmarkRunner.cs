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

namespace AdventOfCode;

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
        _puzzle.Run(_rawInput!, out var p1, out var p2);
        return (p1.ToString()??"", p2.ToString()??"");
    }
}