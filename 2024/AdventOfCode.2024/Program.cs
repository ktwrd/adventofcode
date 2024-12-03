using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AdventOfCode.TwentyTwentyFour;

internal class Program
{
    static void Main(string[] args)
    {
        if (!Directory.Exists("./data"))
        {
            Console.Error.WriteLine("Missing data directory! Please put the input data in a .txt file that has the name of the day.\n" +
                                    "E.g; Day 1 data: ./data/1.txt\n" +
                                    "     Day 2 data: ./data/2.txt\n" +
                                    "     etc...");
            Environment.Exit(1);
        }
        if (args.Length < 1)
        {
            Console.WriteLine($"Enter day (1-31):");
            var line = Console.ReadLine();
            if (int.TryParse(line, out var i))
            {
                Load(i);
            }
            else
            {
                Console.WriteLine($"Could not parse \"{line}\" into an integer");
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
                Console.WriteLine($"Could not parse \"{args[0]}\" into an integer");
            }
        }
    }

    private static Dictionary<int, IDayHandler> FindDayHandlers(params Assembly[] asmArray)
    {
        var result = new Dictionary<int, IDayHandler>();
        foreach (var asm in asmArray)
        {
            foreach (var type in asm.GetTypes())
            {
                if (!typeof(IDayHandler).IsAssignableFrom(type))
                    continue;
                if (!type.IsClass)
                    continue;
                var ctor = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                    null, Type.EmptyTypes, null);
                if (ctor == null)
                {
                    Trace.TraceError($"[FindDayHandlers] No parameterless constructor for {type.AssemblyQualifiedName}");
                    continue;
                }

                var dayAttr = type.GetCustomAttribute<DefaultValueAttribute>();
                if (dayAttr == null)
                {
                    Trace.TraceError($"[FindDayHandlers] Cannot assume day since {nameof(DefaultValueAttribute)} is missing");
                    continue;
                }

                if (int.TryParse(dayAttr!.Value?.ToString(), out var x))
                {
                    var instance = Activator.CreateInstance(type);
                    result[x] = (IDayHandler)instance!;
                }
                else
                {
                    Trace.TraceError($"[FindDayHandlers] Invalid value \"{dayAttr.Value}\" on {nameof(DefaultValueAttribute)}");
                    continue;
                }
            }
        }

        return result;
    }

    private static void Load(int day, string[] content)
    {
        var handlers = FindDayHandlers(typeof(Program).Assembly);
        if (handlers.TryGetValue(day, out var handler))
        {
            handler.Run(content);
        }
        else
        {
            Console.Error.WriteLine($"Solution has not been implemented for day {day}.");
            Environment.Exit(1);
            return;
        }
    }
    private static void Load(int day)
    {
        if (day is < 1 or > 31)
        {
            Trace.WriteLine($"Invalid parameter \"{nameof(day)}\" ({day})");
            Console.Error.WriteLine("Invalid day. Must be >=1 and <=31");
            Environment.Exit(1);
        }

        var possibleFiles = new List<string>()
        {
            Path.Join("data", $"{day}.txt"),
            Path.Join("data", day.ToString().PadLeft(2, '0') + ".txt")
        };
        if (TryGetCustomDataPath(out var customUserPath))
        {
            Trace.WriteLine($"{nameof(possibleFiles)}: Inserted custom user defined path: {customUserPath}");
            possibleFiles.Insert(0, customUserPath!);
        }
        var content = Array.Empty<string>();
        bool found = false;
        foreach (var fn in possibleFiles)
        {
            if (File.Exists(fn))
            {
                Trace.WriteLine($"Loading content from {fn}");
                content = File.ReadAllLines(fn);
                Trace.WriteLine($"Read {content.Length} lines");
                found = true;
                break;
            }
        }

        if (!found)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Could not find input data for the specified day ({day})");
            sb.AppendLine($"Working Directory: {Directory.GetCurrentDirectory()}");
            sb.AppendLine("========== The following locations were checked ==========");
            foreach (var x in possibleFiles)
            {
                sb.AppendLine("- " + x);
            }

            sb.AppendLine();
            sb.AppendLine("Note: Custom input data location can be specified");
            sb.AppendLine("      with the environment variable \"INPUT_PATH\"");
            Console.Error.WriteLine(sb.ToString());
            Environment.Exit(1);
        }

        Load(day, content);
    }

    private static bool TryGetCustomDataPath(out string? path)
    {
        path = null;
        var env = Environment.GetEnvironmentVariable("INPUT_PATH");
        if (!string.IsNullOrEmpty(env))
        {
            if (File.Exists(env))
            {
                path = env;
                return true;
            }
        }

        return false;
    }
}
