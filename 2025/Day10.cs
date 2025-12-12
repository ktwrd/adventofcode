using System.Reflection.PortableExecutable;

namespace AdventOfCode.TwentyTwentyFive;

[Advent(2025, 10)]
public class Day10 : IDayHandler
{
    public void Run(string[] content, out object partOne, out object partTwo)
    {
        long partOneValue = 0;
        long partTwoValue = 0;
        #if DEBUG
        Console.WriteLine($"{content.Length} lines to process");
        #endif
        var data = content.Select(e =>
            {
                var split = e.Split(' ');
                return new MachineCtx(
                    split[0][1..^1].Select(e => e == '#').ToArray(),
                    split[1..^1].Select(e => e[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray(),
                    split[^1][1..^1].Split(',').Select(int.Parse).ToArray()
                );
            }).ToArray();
        partOneValue = data.Sum(e => SolvePartOne(e.IndicatorLights, e.ButtonWiring));
        partTwoValue = data.Sum(SolvePartTwo);

        partOne = partOneValue;
        partTwo = partTwoValue;
    }

    private static int SolvePartOne(bool[] indicatorLights, int[][] buttonWiring)
    {
        var target = string.Join("", indicatorLights.Select(e => e ? '#' : '.'));
        var reachable = new Dictionary<string, int>();
        var init = new string(Enumerable.Range(0, target.Length).Select(e => '.').ToArray());
        reachable[init] = 0;
        var queue = new Queue<string>();
        queue.Enqueue(init);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (item.Equals(target))
            {
                return reachable[item];
            }
            foreach (var button in buttonWiring)
            {
                var toggled = string.Join("", item.ToCharArray()
                    .Select((y, i) =>
                    {
                        if (button.Contains((int)i))
                        {
                            return y == '#' ? '.' : '#';
                        }
                        return y;
                    }));
                if (!reachable.ContainsKey(toggled))
                {
                    reachable[toggled] = reachable[item] + 1;
                    queue.Enqueue(toggled);
                }
            }
        }
        throw new Exception();
    }

    private void SolvePartTwoLogic(ref MachineCtx machine, ref int[] state, (int, int[][]) data)
    {
        if (state.Any(e => e < 0)) return;

        var (sofar, buttons) = data;
        var stepsLeft = 0;
        for (int i = 0; i < state.Length; i++)
        {
            if (stepsLeft < state[i])
            {
                stepsLeft = state[i];
            }
        }
        if (stepsLeft == 0)
        {
            if (machine.PartTwoBest == -1 || machine.PartTwoBest > sofar)
            {
                machine.PartTwoBest = sofar;
            }
            return;
        }
        if (machine.PartTwoBest != -1 && sofar + stepsLeft >= machine.PartTwoBest) return;

        for (int ii = 0; ii < state.Length; ii++)
        {
            for (int jj = 0; jj < state.Length; jj++)
            {
                if (state[ii] > state[jj])
                {
                    var f = buttons.Where((e) => e.Contains(ii) && !e.Contains(jj)).ToArray();
                    if (f.Length < 1)
                    {
                        return;
                    }
                    if (f.Length == 1)
                    {
                        var b = PressButton(ref state, ref f[0]);
                        SolvePartTwoLogic(ref machine, ref b, (sofar + 1, buttons));
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            var b = PressButton(ref state, ref buttons[i]);
            SolvePartTwoLogic(ref machine, ref b, (sofar + 1, buttons.Skip(i).ToArray()));
        }
    }

    private long SolvePartTwo(MachineCtx machine)
    {
        SolvePartTwoLogic(ref machine, ref machine.Joltage, (0, machine.ButtonWiring));
        return machine.PartTwoBest;
    }

    private static int[] PressButton(ref int[] state, ref int[] button)
    {
        var toggle = new int[state.Length];
        for (int i = 0; i < state.Length; i++)
        {
            toggle[i] = (int)(state[i] - button.Count(e => e == i));
        }
        return toggle;
    }

    private static string MachineToString(bool[] indicatorLights, int[][] buttonWiring, int[] joltage)
    {
        var a = new string([..indicatorLights.Select(e => e ? '#' : '.')]);
        var b = string.Join(' ', buttonWiring.Select(e => "(" + string.Join(',', e) + ")"));
        var c = string.Join(',', joltage);
        return $"[{a}] {b} {{{c}}}";
    }

    internal class MachineCtx(bool[] indicatorLights, int[][] buttonWiring, int[] joltage)
    {
        public bool[] IndicatorLights = indicatorLights;
        public int[][] ButtonWiring = buttonWiring;
        public int[] Joltage = joltage;
        public long PartTwoBest = -1;
    }
}