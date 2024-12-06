using System.Collections.Frozen;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(6)]
public class DaySix : IDayHandler
{
    public void Run(string[] lines)
    {
	    var a = PartOne(lines);
	    Console.WriteLine($"Part One: {a}");
	    var b = PartTwo(lines);
	    Console.WriteLine($"Part Two: {b}");
    }

    public int PartTwo(string[] lines)
    {
	    int width = lines[0].Length;
	    int height = lines.Length;
	    var allPoints = new List<Point>();
	    GetMapInfo(lines, out var ogObstacles, out var guardPosition, out var guardDir);
	    for (int i = 0; i < height; i++)
	    {
		    for (int c = 0; c < width; c++)
		    {
			    var p = new Point(c, i);
			    if (!ogObstacles.Contains(p))
			    {
				    allPoints.Add(new(c, i));
			    }
		    }
	    }
	    
	    int result = 0;
	    int count = 0;
		void Force(Point point)
		{
			var obstacles = ogObstacles.ToList();
			obstacles.Add(point);

			var (cm, reachedLimit) = Process(obstacles, guardPosition, guardDir);
			cm.Clear();
			if (reachedLimit)
			{
				result++;
			}

			count++;
			if (count % 100 == 0)
			{
				Console.WriteLine(count);
			}
		}
		
		Console.WriteLine($"Brute-forcing {allPoints.Count} spots.");
	    Parallel.ForEach(allPoints, Force);
	    
	    return result;
    }

    public int PartOne(string[] lines)
    {
	    var (count, l) = Process(lines);
	    return count;
    }

    public (int, bool) Process(string[] lines)
    {
	    GetMapInfo(lines, out var obstacles, out var currentGuardPosition, out var currentGuardDirection);

	    var (walkedPositions, reachedLimit) = Process(obstacles, currentGuardPosition, currentGuardDirection);
	    
	    walkedPositions = walkedPositions.Distinct().ToList();
	    var walkedMap = GenerateMap(lines[0].Length, lines.Length, obstacles, Point.Empty,  ' ', walkedPositions);
	    Console.WriteLine($"========= Touched parts");
	    int count = 0;
	    foreach (var x in walkedMap)
	    {
		    count += Regex.Matches(x, "(X)").Count;
	    }

	    return (count, reachedLimit);
    }

    public (List<Point>, bool) Process(List<Point> obstacles, Point currentGuardPosition, char currentGuardDirection)
    {
	    bool cont = true;
	    Point guardPosition = currentGuardPosition;
	    char guardDirection = currentGuardDirection;
	    bool reachedLimit = false;
	    var hs = new HashSet<(Point, char)>();
	    hs.Add((currentGuardPosition, currentGuardDirection));
	    while (cont)
	    {
		    var s = ProcessGuardStepAlt(
			    obstacles,
			    guardPosition,
			    guardDirection,
			    out var outGuardMove,
			    out var outGuardDirection,
			    out var outGuardPos);
		    if (s == false || outGuardPos.X < 0 || outGuardPos.Y < 0)
		    {
			    cont = false;
		    }

		    if (outGuardPos.X >= 0 && outGuardPos.Y >= 0)
		    {
			    if (hs.Contains((outGuardPos, outGuardDirection)))
			    {
				    reachedLimit = true;
				    cont = false;
			    }
			    else
			    {
				    hs.Add((outGuardPos, outGuardDirection));
			    }

			    guardPosition = outGuardPos;
			    guardDirection = outGuardDirection;
		    }
	    }

	    return (hs.Select(e => e.Item1).ToList(), reachedLimit);
    }
    
    public static FrozenDictionary<char, char> DirectionMap => new Dictionary<char, char>()
    {
	    { '^', 'N' },
	    { 'v', 'S' },
	    { '<', 'W' },
	    { '>', 'E' }
    }.ToFrozenDictionary();

    public static FrozenDictionary<char, char> DirectionMapInverse => new Dictionary<char, char>()
    {
	    { 'N', '^' },
	    { 'S', 'v' },
	    { 'W', '<' },
	    { 'E', '>' }
    }.ToFrozenDictionary();
    public void GetMapInfo(string[] lines, out List<Point> obstacles, out Point guardPosition, out char guardDirection)
    {
	    obstacles = new List<Point>();
	    guardPosition = new(-1, -1);
	    guardDirection = '?';
	    for (int i = 0; i < lines.Length; i++)
	    {
		    for (int c = 0; c < lines[i].Length; c++)
		    {
			    if (lines[i][c] == '#')
			    {
				    obstacles.Add(new Point(c, i));
			    }
			    else if (DirectionMap.TryGetValue(lines[i][c], out var dir))
			    {
				    guardPosition = new Point(c, i);
				    guardDirection = dir;
			    }
		    }
	    }
    }

    public bool ProcessGuardStepAlt(
	    List<Point> obstacles,
	    Point currentGuardPosition,
	    char currentGuardDirection,
	    out bool guardMove,
	    out char guardDir,
	    out Point guardPos)
    {
	    guardMove = false;
	    guardDir = currentGuardDirection;
	    guardPos = new(currentGuardPosition.X, currentGuardPosition.Y);

	    if (guardPos.X < 0 || guardPos.Y < 0)
	    {
		    return false;
	    }
	    
	    Point? currentObstacle = null;
	    foreach (var obs in obstacles)
	    {
		    if (currentObstacle != null)
			    continue;
		    switch (guardDir)
		    {
			    case 'N':
				    if (obs.Y == guardPos.Y - 1 && obs.X == guardPos.X)
				    {
					    currentObstacle = obs;
				    }
				    break;
			    case 'S':
				    if (obs.Y == guardPos.Y + 1 && obs.X == guardPos.X)
				    {
					    currentObstacle = obs;
				    }
				    break;
			    case 'E':
				    if (obs.Y == guardPos.Y && obs.X == guardPos.X + 1)
				    {
					    currentObstacle = obs;
				    }
				    break;
			    case 'W':
				    if (obs.Y == guardPos.Y && obs.X == guardPos.X - 1)
				    {
					    currentObstacle = obs;
				    }
				    break;
		    }
	    }

	    if (currentObstacle == null)
	    {
		    switch (guardDir)
		    {
			    case 'N':
				    guardPos.Y--;
				    break;
			    case 'S':
				    guardPos.Y++;
				    break;
			    case 'E':
				    guardPos.X++;
				    break;
			    case 'W':
				    guardPos.X--;
				    break;
		    }

		    guardMove = true;
	    }
	    else
	    {
		    if (RotationDictionary.TryGetValue(guardDir, out var sp))
		    {
			    guardDir = sp;
		    }
	    }

	    return true;
    }
    public static FrozenDictionary<char, char> RotationDictionary => new Dictionary<char, char>()
    {
	    { 'N', 'E' },
	    { 'S', 'W' },
	    { 'E', 'S' },
	    { 'W', 'N' }
    }.ToFrozenDictionary();

    private string[] GenerateMap(
	    int width,
	    int height,
	    List<Point> obstacles,
	    Point guardPos,
	    char guardDirection,
	    List<Point>? guardWalkedPositions = null)
    {
	    var result = new string[height];
	    for (int i = 0; i < height; i++)
	    {
		    var tmp = "".PadRight(width, '.').ToCharArray();
		    for (int c = 0; c < tmp.Length; c++)
		    {
			    if (guardPos.X == c && guardPos.Y == i)
			    {
				    if (DirectionMapInverse.TryGetValue(guardDirection, out var xpm))
				    {
					    tmp[c] = xpm;
				    }
			    }

			    foreach (var obs in obstacles)
			    {
				    if (obs.X == c && obs.Y == i)
				    {
					    tmp[c] = '#';
				    }
			    }

			    if (guardWalkedPositions != null)
			    {
				    foreach (var pt in guardWalkedPositions)
				    {
					    if (pt.X == c && pt.Y == i)
					    {
						    tmp[c] = 'X';
					    }
				    }
			    }
		    }

		    result[i] = new string(tmp);
	    }

	    return result;
    }
}