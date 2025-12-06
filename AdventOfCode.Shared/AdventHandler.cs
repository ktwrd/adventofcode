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
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using NeoSmart.PrettySize;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AdventOfCode;

public class AdventHandler
{
    public AdventHandler(DirectoryInfo dataDirectory)
    {
        DataDirectory = dataDirectory;
        RegisteredTypes = [];
        var entryAsm = Assembly.GetEntryAssembly();
        if (entryAsm != null)
        {
            RegisterTypes(entryAsm.GetTypes());
        }
    }

    public DirectoryInfo DataDirectory { get; private set; }
    public IReadOnlyList<AdventRegisteredType> RegisteredTypes { get; private set; }

    public record AdventRegisteredType
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public required Type Type { get; init; }
        public required AdventAttribute Attribute { get; init; }

        internal string DistinctIdentifier => $"{Attribute.Year},{Attribute.Day}";

        public IDayHandler CreateInstance()
        {
            if (Activator.CreateInstance(Type) is not IDayHandler handler)
                throw new InvalidOperationException($"{Type} does not implement {typeof(IDayHandler)}");
            return handler;
        }
    }

    public void RegisterTypes(
        params IEnumerable<Type> types)
    {
        var registeredTypes = new List<AdventRegisteredType>(RegisteredTypes);
        foreach (var type in types)
        {
            if (!type.IsClass || !typeof(IDayHandler).IsAssignableFrom(type))
                continue;
            var attr = type.GetCustomAttribute<AdventAttribute>();
            if (attr == null) continue;

            if (!registeredTypes.Any(e => e.Attribute.ValueEquals(attr)))
            {
                registeredTypes.Add(new AdventRegisteredType
                {
                    Type = type,
                    Attribute = attr
                });
            }
        }
        RegisteredTypes = registeredTypes.DistinctBy(e => e.DistinctIdentifier).ToList();
    }

    /// <summary>
    /// Get the input data for the provided advent registered type.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no files could be found.</exception>
    public static string[] GetData(ref AdventRegisteredType type)
    {
        var filenames = new[]
        {
            $"{type.Attribute.Day}.txt",
            $"{type.Attribute.Day.ToString().PadLeft(2, '0')}.txt",
            $"{type.Attribute.Year}-{type.Attribute.Day}.txt",
            $"{type.Attribute.Year}-{type.Attribute.Day.ToString().PadLeft(2, '0')}.txt",
        };
        string? targetFilename = null;
        foreach (var filename in filenames)
        {
            var location = Path.Join("./data/", filename);
            if (File.Exists(location))
            {
                targetFilename = location;
                break;
            }
        }
        if (targetFilename == null)
            throw new InvalidOperationException($"Could not find data for identifier \"{type.DistinctIdentifier}\". Attempted the following locations in ./data/\n" + string.Join("\n", filenames));
        return File.ReadAllLines(targetFilename);
    }

    public void Execute(ref AdventRegisteredType type)
    {
        Execute(ref type, GetData(ref type));
    }
    public static void Execute(ref AdventRegisteredType type, string[] content)
    {
        Console.WriteLine($"[Perf] Executing year={type.Attribute.Year},day={type.Attribute.Day}");
        var instance = type.CreateInstance();
        
        GC.Collect(2, GCCollectionMode.Forced, true);
        GC.WaitForPendingFinalizers();
        GC.RefreshMemoryLimit();

        var allocStart = GC.GetTotalAllocatedBytes();
        var sw = Stopwatch.StartNew();

        instance.Run(content, out var p1, out var p2);

        sw.Stop();
        Console.WriteLine($"[Advent] p1={p1}, p2={p2}");
        GC.Collect(2, GCCollectionMode.Forced, true);
        GC.WaitForPendingFinalizers();
        GC.RefreshMemoryLimit();

        var alloc = GC.GetTotalAllocatedBytes() - allocStart;
        Console.WriteLine($"[Perf] Memory Allocated: {PrettySize.Bytes(alloc)}");
        Console.WriteLine($"[Perf] Took: {sw.Elapsed.TotalMilliseconds}ms");

        //TODO find an easier way to do this. currently refuses to work when AdventBenchmarkRunner is in AdventOfCode.Shared
        //Console.WriteLine("Should benchmark? (1 = yes)");
        //var shouldBenchmark = Console.ReadLine() == "1";
        //if (shouldBenchmark)
        //{
        //    BenchmarkSwitcher
        //        .FromTypes([typeof(AdventBenchmarkRunner<>).MakeGenericType(type.Type)])
        //        .Run();
        //}
    }
}
