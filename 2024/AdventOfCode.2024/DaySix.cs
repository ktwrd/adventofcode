using System.Collections.Frozen;
using System.ComponentModel;
using System.Diagnostics;
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

    #region Part Two
    public int PartTwo(IReadOnlyCollection<string> inputData)
    {
	    var possibilities = GetPartTwoPossibilities(inputData);
	    
	    int loopCount = 0;
	    var data = MapData(inputData);
	    foreach (var possibility in possibilities)
	    {
		    data[possibility.X, possibility.Y] = '#';
		    var guard = new GuardObject();
		    guard.ParseFromMap(inputData);
		    var x = CheckForLoop(data, guard);
		    if (x)
		    {
			    loopCount++;
		    }
		    data[possibility.X, possibility.Y] = '.';
	    }
		
	    return loopCount;
    }

    private GuardDirection[,] InitDirections(char[,] inputData)
    {
	    int width = inputData.GetLength(0);
	    int height = inputData.GetLength(1);
	    var result = new GuardDirection[width, height];
	    for (int x = 0; x < width; x++)
	    {
		    for (int y = 0; y < height; y++)
		    {
			    result[x, y] = GuardDirection.None;
		    }
	    }

	    return result;
    }

    private bool CheckForLoop(
	    char[,] inputData,
	    GuardObject guard)
    {
	    var visited = InitDirections(inputData);
	    var x = guard.Position.X;
	    var y = guard.Position.Y;
	    int width = inputData.GetLength(0);
	    int height = inputData.GetLength(1);
	    while (true)
	    {
		    var dir = visited[x, y];
		    if (dir.HasFlag(guard.Direction))
		    {
			    return true;
		    }

		    visited[x, y] |= guard.Direction;

		    var nextPosition = new Point(x + guard.MoveDelta.X, y + guard.MoveDelta.Y);
		    if (nextPosition.X < 0 || nextPosition.X >= width)
			    break;
		    if (nextPosition.Y < 0 || nextPosition.Y >= height)
			    break;
		    var nextChar = inputData[nextPosition.X, nextPosition.Y];
		    if (nextChar == '*')
			    break;
		    if (nextChar == '#')
		    {
			    guard.Turn();
			    continue;
		    }

		    x = nextPosition.X;
		    y = nextPosition.Y;
	    }

	    return false;
    }

    private char[,] MapData(IReadOnlyCollection<string> inputData)
    {
	    int height = inputData.Count;
	    int width = inputData.First().Length;
	    var result = new char[width, inputData.Count];
	    for (int y = 0; y < height; y++)
	    {
		    var row = inputData.ElementAt(y);
		    for (int x = 0; x < width; x++)
		    {
			    result[x, y] = row[x];
		    }
	    }

	    return result;
    }
    private static Point GetDelta(GuardDirection direction)
    {
	    if (direction.HasFlag(GuardDirection.Up))
	    {
		    return new Point(0, -1);
	    }
	    else if (direction.HasFlag(GuardDirection.Down))
	    {
		    return new Point(0, 1);
	    }
	    else if (direction.HasFlag(GuardDirection.Left))
	    {
		    return new Point(-1, 0);
	    }
	    else if (direction.HasFlag(GuardDirection.Right))
	    {
		    return new Point(1, 0);
	    }

	    return Point.Empty;
    }

    public struct GuardObject
    {
	    public GuardObject()
	    {
		    Direction = GuardDirection.None;
		    Position = Point.Empty;
		    MoveDelta = Point.Empty;
	    }

	    public void ParseFromMap(IReadOnlyCollection<string> inputData)
	    {
		    int width = inputData.First().Length;
		    int height = inputData.Count;
		    for (int y = 0; y < height; y++)
		    {
			    var row = inputData.ElementAt(y);
			    for (int x = 0; x < width; x++)
			    {
				    if (row[x] == '^')
				    {
					    Direction = GuardDirection.Up;
					    Position = new Point(x, y);
					    MoveDelta = GetDelta(Direction);
				    }
				    else if (row[x] == '>')
				    {
					    Direction = GuardDirection.Right;
					    Position = new Point(x, y);
					    MoveDelta = GetDelta(Direction);
				    }
				    else if (row[x] == 'v' || row[x] == 'V')
				    {
					    Direction = GuardDirection.Down;
					    Position = new Point(x, y);
					    MoveDelta = GetDelta(Direction);
				    }
				    else if (row[x] == '<')
				    {
					    Direction = GuardDirection.Left;
					    Position = new Point(x, y);
					    MoveDelta = GetDelta(Direction);
				    }
			    }
		    }
	    }
	    public GuardDirection Direction;
	    public Point Position;

	    public Point MoveDelta;
	    
	    public void Turn()
	    {
		    if (Direction == GuardDirection.Up)
		    {
			    Direction = GuardDirection.Right;
			    MoveDelta = GetDelta(Direction);
		    }
		    else if (Direction == GuardDirection.Down)
		    {
			    Direction = GuardDirection.Left;
			    MoveDelta = GetDelta(Direction);
		    }
		    else if (Direction == GuardDirection.Left)
		    {
			    Direction = GuardDirection.Up;
			    MoveDelta = GetDelta(Direction);
		    }
		    else if (Direction == GuardDirection.Right)
		    {
			    Direction = GuardDirection.Down;
			    MoveDelta = GetDelta(Direction);
		    }
		    else
		    {
			    // Direction is invalid!
			    Debugger.Break();
		    }
	    }
    }

	[Flags]
    public enum GuardDirection
    {
	    None = 0x00,
	    Up = 0x01,
	    Down = 0x02,
	    Left = 0x04,
	    Right = 0x08,
	    All = Up | Down | Left | Right
    }

    private List<Point> GetPartTwoPossibilities(IReadOnlyCollection<string> inputData)
    {
	    var result = new List<Point>();
	    int height = inputData.Count;
	    var width = inputData.First().Length;
	    for (int y = 0; y < height; y++)
	    {
		    var row = inputData.ElementAt(y);
		    for (int x = 0; x < width; x++)
		    {
			    if (row[x] == '.')
			    {
				    result.Add(new Point(x, y));
			    }
		    }
	    }

	    return result;
    }
    #endregion
    
    #region Part One
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
	    { 'V', 'S'},
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
    #endregion
}