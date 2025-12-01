using System.Diagnostics;
using System.Reflection;
using NeoSmart.PrettySize;

namespace AdventOfCode;

public static class StartupHelper
{
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
        if (args.Length == 0)
        {
            Console.WriteLine($"Enter day (1-31):");
            var value = Console.ReadLine();
            if (int.TryParse(value, out var valueInt))
            {
                FindAndExecute(year, valueInt);
            }
            else
            {
                Console.WriteLine($"Could not parse \"{value}\" into an integer");
            }
        }
        else
        {
            if (int.TryParse(args[0], out var valueInt))
            {
                FindAndExecute(year, valueInt);
            }
            else
            {
                Console.WriteLine($"Could not parse \"{args[0]}\" into an integer");
            }
        }
    }
    public static void FindAndExecute(int year, int dayIdent)
    {
        var locations = new[]
        {
            $"./data/{dayIdent}.txt",
            "./data/" + dayIdent.ToString().PadLeft(2, '0') + ".txt",
            $"./data/{year}-{dayIdent}.txt",
            "./data/" + year.ToString().PadLeft(0, '0') + "-" + dayIdent.ToString().PadLeft(2, '0') + ".txt",
        };
        string? targetFilename = null;
        foreach (var location in locations)
        {
            if (File.Exists(location))
            {
                targetFilename = location;
                Console.WriteLine($"[{nameof(StartupHelper)}] Using file: {targetFilename}");
                break;
            }
        }
        if (targetFilename == null)
            throw new InvalidOperationException($"Could not find viable path for input data! Tried:\n" + string.Join("\n", locations));
        var content = File.ReadAllLines(targetFilename);
        FindAndExecute(year, dayIdent, content);
    }

    public static void FindAndExecute(int year, int dayIdent, string[] content)
    {
        var handler = FindHandler(year, dayIdent);
        
        var allocStart = GC.GetTotalAllocatedBytes();
        var sw = Stopwatch.StartNew();
        handler.Run(content);
        sw.Stop();
        var ms = sw.ElapsedMilliseconds;
        var alloc = GC.GetTotalAllocatedBytes() - allocStart;
        Console.WriteLine("[Perf] Allocated: " + PrettySize.Bytes(alloc));
        Console.WriteLine($"[Perf] Took: {ms}ms");
    }

    public static IDayHandler FindHandler(int year, int dayIdent)
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in asm.GetTypes())
            {
                if (!typeof(IDayHandler).IsAssignableFrom(type) || !type.IsClass)
                    continue;

                var attr = type.GetCustomAttribute<AdventAttribute>();
                if (attr == null)
                    continue;
                if (attr.Year != year || attr.Day != dayIdent)
                    continue;

                var ctor = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, Type.EmptyTypes, null);
                if (ctor == null)
                    continue;
                
                if (Activator.CreateInstance(type) is IDayHandler handler)
                    return handler;
            }
        }
        throw new InvalidOperationException($"Could not find IDayHandler for Year {year} and Day {dayIdent}");
    }
}