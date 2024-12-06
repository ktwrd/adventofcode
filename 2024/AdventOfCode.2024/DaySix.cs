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
	    GetMapInfo(lines, out var obstacles, out var currentGuardPosition, out var currentGuardDirection);
	    Console.WriteLine($"Obstacles:");
	    Console.WriteLine(string.Join("\n", obstacles.Select(e => e.ToString())));
	    Console.WriteLine($"Guard Position: {currentGuardPosition}");
	    Console.WriteLine($"Guard Direction: {currentGuardDirection}");

	    bool cont = true;
	    var mapState = lines;
	    var stepCount = 0;
	    var walkedPositions = new List<Point>();
	    while (cont)
	    {
		    var s = ProcessGuardStep(mapState, out var updated, out bool guardMoved, out var guardPos);
		    /*Console.WriteLine($"======== {stepCount}");
		    foreach (var x in updated)
		    {
			    Console.WriteLine(x);
		    }*/
		    if (s == false)
		    {
			    cont = false;
		    }
		    else
		    {
			    if (guardMoved)
			    {
				    walkedPositions.Add(guardPos);
				    stepCount++;
			    }

			    mapState = updated;
		    }
	    }

	    walkedPositions = walkedPositions.Distinct().ToList();
	    var walkedMap = GenerateMap(mapState[0].Length, mapState.Length, obstacles, Point.Empty,  ' ', walkedPositions);
	    Console.WriteLine($"========= Touched parts ({walkedPositions.Count})");
	    int count = 0;
	    foreach (var x in walkedMap)
	    {
		    count += Regex.Matches(x, "(X)").Count;
		    Console.WriteLine(x);
	    }

	    Console.WriteLine(count);
    }

    public static FrozenDictionary<char, char> DirectionMap = new Dictionary<char, char>()
    {
	    { '^', 'N' },
	    { 'v', 'S' },
	    { '<', 'W' },
	    { '>', 'E' }
    }.ToFrozenDictionary();

    public static FrozenDictionary<char, char> DirectionMapInverse = new Dictionary<char, char>()
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
			    else if (DirectionMap.TryGetValue(lines[i][c], out var xpfps))
			    {
				    guardPosition = new Point(c, i);
				    guardDirection = xpfps;
			    }
		    }
	    }
    }

    public bool ProcessGuardStep(string[] currentState, out string[] updatedMap, out bool guardMoved, out Point guardPosition)
    {
	    guardMoved = false;
	    GetMapInfo(currentState, out var obstacles, out var currentGuardPosition, out var currentGuardDirection);
	    if (currentGuardPosition.X == -1 && currentGuardPosition.Y == -1)
	    {
		    guardPosition = new(-1, -1);
		    updatedMap = [];
		    Console.WriteLine($"Guard OOB!");
		    return false;
	    }

	    Point? infrontOfObstacle = null;
	    foreach (var obs in obstacles)
	    {
		    if (infrontOfObstacle != null)
			    continue;
		    switch (currentGuardDirection)
		    {
			    case 'N':
				    if (obs.Y == currentGuardPosition.Y - 1 && obs.X == currentGuardPosition.X)
				    {
					    infrontOfObstacle = obs;
				    }
				    break;
			    case 'S':
				    if (obs.Y == currentGuardPosition.Y + 1 && obs.X == currentGuardPosition.X)
				    {
					    infrontOfObstacle = obs;
				    }
				    break;
			    case 'E':
				    if (obs.Y == currentGuardPosition.Y && obs.X == currentGuardPosition.X + 1)
				    {
					    infrontOfObstacle = obs;
				    }
				    break;
			    case 'W':
				    if (obs.Y == currentGuardPosition.Y && obs.X == currentGuardPosition.X - 1)
				    {
					    infrontOfObstacle = obs;
				    }
				    break;
		    }
	    }

	    if (infrontOfObstacle == null)
	    {
		    switch (currentGuardDirection)
		    {
			    case 'N':
				    currentGuardPosition.Y--;
				    break;
			    case 'S':
				    currentGuardPosition.Y++;
				    break;
			    case 'E':
				    currentGuardPosition.X++;
				    break;
			    case 'W':
				    currentGuardPosition.X--;
				    break;
		    }

		    guardMoved = true;
	    }
	    else
	    {
		    if (RotationDictionary.TryGetValue(currentGuardDirection, out var sp))
		    {
			    currentGuardDirection = sp;
		    }
	    }
	    updatedMap = GenerateMap(
		    currentState[0].Length, 
		    currentState.Length,
		    obstacles,
		    currentGuardPosition,
		    currentGuardDirection);
	    guardPosition = currentGuardPosition;
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