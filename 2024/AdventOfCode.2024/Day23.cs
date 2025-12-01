using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(23)]
public class Day23 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    public Dictionary<string, List<string>> ParseInput(string[] inputData)
    {
        var links = new Dictionary<string, List<string>>();
        foreach (var line in inputData)
        {
            var split = line.Split('-');
            var f = split[0];
            var t = split[1];
            if (!links.ContainsKey(f))
            {
                links[f] = [];
            }

            if (!links.ContainsKey(t))
            {
                links[t] = [];
            }
            links[f].Add(t);
            links[t].Add(f);
        }

        return links;
    }
    
    private long PartOne(string[] inputData)
    {
        var links = ParseInput(inputData);

        foreach (var k in links.Keys)
        {
            links[k] = links[k].Distinct().ToList();
        }

        var lookup = new List<string[]>();
        foreach (var (node, siblings) in links)
        {
            for (int i = 0; i < siblings.Count; i++)
            {
                for (int j = i + 1; j < siblings.Count; j++)
                {
                    var n1 = siblings[i];
                    var n2 = siblings[j];
                    if (links.ContainsKey(n1))
                    {
                        if (links[n1].Contains(n2))
                        {
                            lookup.Add(new[] {node, n1, n2}.OrderBy(e => e).ToArray());
                        }
                    }
                }
            }
        }



        return lookup.DistinctBy(e => string.Join(',', e)).LongCount(e => e.Any(x => x.StartsWith('t')));
    }

    private string PartTwo(string[] inputData)
    {
        var links = ParseInput(inputData);

        var maxClique = new List<string>();
        PartTwoFind([], links.Keys.ToList(), [], links, maxClique);
        return string.Join(',', maxClique.OrderBy(e => e));
    }

    private void PartTwoFind(List<string> clique, List<string> options, List<string> excluded,
        Dictionary<string, List<string>> graph, List<string> maxClique)
    {
        if (options.Count == 0 && excluded.Count == 0)
        {
            if (clique.Count > maxClique.Count)
            {
                maxClique.Clear();
                maxClique.AddRange(clique);
            }

            return;
        }

        var optsArr = options.ToArray();
        foreach (var vertex in optsArr)
        {
            clique.Add(vertex);
            var siblings = new List<string>();
            if (graph.ContainsKey(vertex))
            {
                siblings.AddRange(graph[vertex]);
            }
            
            PartTwoFind(
                clique,
                options.Where(e => siblings.Contains(e)).ToList(),
                excluded.Where(e => siblings.Contains(e)).ToList(),
                graph,
                maxClique);
            clique.Remove(vertex);
            options.Remove(vertex);
            excluded.Remove(vertex);
        }
    }
}