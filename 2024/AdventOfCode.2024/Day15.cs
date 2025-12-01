using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(15)]
public class Day15 : IDayHandler
{
    public void Run(string[] inputData)
    {
        var a = PartOne(inputData);
        Console.WriteLine($"Part One: {a}");
        var b = PartTwo(inputData);
        Console.WriteLine($"Part Two: {b}");
    }

    public long PartOne(string[] inputData)
    {
        var (warehouse, movements) = Parse(inputData.ToList());
        foreach (var move in movements)
        {
            var robotPosition = FindRobotPosition(warehouse);
            if (robotPosition is { X: -1, Y: -1 })
            {
                throw new InvalidDataException($"Could not find robot in warehouse!");
            }

            var offset = DirectionOffset[move];
            _movementChanges = [];
            if (CanMoveInDirection(robotPosition, offset, warehouse))
            {
                foreach (var (p, x) in _movementChanges)
                {
                    warehouse[p.X, p.Y] = x;
                }
            }
        }
        
        return GetResult(warehouse);
    }

    private long GetResult(WarehouseObject[,] warehouse)
    {
        double result = 0;
        for (var x = 0; x < warehouse.GetLength(0); x++)
        {
            for (var y = 0; y < warehouse.GetLength(1); y++)
            {
                var w = warehouse[x, y];
                if (w is WarehouseObject.Box or WarehouseObject.BoxLeft)
                {
                    result += (100 * y) + x;
                }
            }
        }
        return Convert.ToInt64(Math.Round(result));
    }
    public long PartTwo(string[] inputData)
    {
        var (warehouse, movements) = Parse(inputData, partTwo: true);
        foreach (var move in movements)
        {
            var robotPosition = FindRobotPosition(warehouse);
            if (robotPosition is { X: -1, Y: -1 })
            {
                throw new InvalidDataException($"Could not find robot in warehouse!");
            }

            var offset = DirectionOffset[move];
            _movementChanges = [];
            if (CanMoveInDirection(robotPosition, offset, warehouse))
            {
                foreach (var (p, x) in _movementChanges)
                {
                    warehouse[p.X, p.Y] = x;
                }
            }
        }
        
        return GetResult(warehouse);
    }

    private Dictionary<Point, WarehouseObject> _movementChanges = [];
    private bool CanMoveInDirection(Point position, Point offset, WarehouseObject[,] warehouse)
    {
        var next = new Point(position.X + offset.X, position.Y + offset.Y);
        if (next.X < 0 || next.X >= warehouse.GetLength(0) || next.Y < 0 || next.Y >= warehouse.GetLength(1))
            return false;
        var nextObject = warehouse[next.X, next.Y];

        if (nextObject == WarehouseObject.Wall) return false;
        
        if (nextObject == WarehouseObject.Box
            || (offset.Y == 0 && (nextObject == WarehouseObject.BoxLeft || nextObject == WarehouseObject.BoxRight)))
        {
            if (CanMoveInDirection(next, offset, warehouse) == false)
                return false;
        }
        else if (nextObject == WarehouseObject.BoxLeft)
        {
            if (CanMoveInDirection(next, offset, warehouse) == false)
                return false;
            if (CanMoveInDirection(new Point(position.X + 1, position.Y + offset.Y), offset, warehouse) == false)
                return false;
        }
        else if (nextObject == WarehouseObject.BoxRight)
        {
            if (CanMoveInDirection(next, offset, warehouse) == false)
                return false;
            if (CanMoveInDirection(new Point(position.X - 1, position.Y + offset.Y), offset, warehouse) == false)
                return false;
        }

        _movementChanges[next] = warehouse[position.X, position.Y];
        if (_movementChanges.ContainsKey(position) == false)
        {
            _movementChanges[position] = WarehouseObject.Empty;
        }
        

        return true;
    }


    private Point FindRobotPosition(WarehouseObject[,] warehouse)
    {
        for (int x = 0; x < warehouse.GetLength(0); x++)
        {
            for (int y = 0; y < warehouse.GetLength(1); y++)
            {
                if (warehouse[x, y] == WarehouseObject.Robot)
                    return new(x, y);
            }
        }

        return new(-1, -1);
    }

    public (WarehouseObject[,] warehouse, Direction[] robotMovements) Parse(IList<string> inputData, bool partTwo = false)
    {
        var warehouseWidth = inputData[0].Length;
        var warehouseHeight = inputData.ToList().IndexOf("");
        var warehouse = new WarehouseObject[warehouseWidth, warehouseHeight];
        var robotMovementString = "";
        for (int y = 0; y < inputData.Count; y++)
        {
            if (y > warehouseHeight)
            {
                robotMovementString += inputData[y].Trim(' ');
            }
            else
            {
                var row = inputData[y].ToCharArray();
                for (int x = 0; x < row.Length; x++)
                {
                    warehouse[x, y] = ParseWarehouseObject(row[x]);
                }
            }
        }

        if (partTwo)
        {
            var w = ParseWarehousePartTwo(warehouse);
            warehouse = w;
        }

        var robotMovements = ParseDirection(robotMovementString.Trim(' ').ToCharArray());
        return (warehouse, robotMovements);
    }

    public WarehouseObject[,] ParseWarehouseObjects(IList<string> inputData)
    {
        var warehouseWidth = inputData[0].Length;
        var warehouseHeight = inputData.ToList().IndexOf("");
        if (warehouseHeight == -1)
            warehouseHeight = inputData.Count;
        var warehouse = new WarehouseObject[warehouseWidth, warehouseHeight];
        for (int y = 0; y < inputData.Count; y++)
        {
            if (y <= warehouseHeight)
            {
                var row = inputData[y].ToCharArray();
                for (int x = 0; x < row.Length; x++)
                {
                    warehouse[x, y] = ParseWarehouseObject(row[x]);
                }
            }
        }

        return warehouse;
    }

    public WarehouseObject[,] ParseWarehousePartTwo(WarehouseObject[,] original)
    {
        var inputData = new List<string>();
        for (int y = 0; y < original.GetLength(1); y++)
        {
            var row = new List<WarehouseObject>();
            for (int x = 0; x < original.GetLength(0); x++)
            {
                switch (original[x, y])
                {
                    case WarehouseObject.Empty:
                        row.Add(WarehouseObject.Empty);
                        row.Add(WarehouseObject.Empty);
                        break;
                    case WarehouseObject.Wall:
                        row.Add(WarehouseObject.Wall);
                        row.Add(WarehouseObject.Wall);
                        break;
                    case WarehouseObject.Robot:
                        row.Add(WarehouseObject.Robot);
                        row.Add(WarehouseObject.Empty);
                        break;
                    case WarehouseObject.Box:
                        row.Add(WarehouseObject.BoxLeft);
                        row.Add(WarehouseObject.BoxRight);
                        break;
                }
            }

            var rowString = "";
            for (int x = 0; x < row.Count; x++)
            {
                var c = row[x];
                if (c == WarehouseObject.Empty)
                {
                    rowString += '.';
                }
                else if (c == WarehouseObject.Wall)
                {
                    rowString += '#';
                }
                else if (c == WarehouseObject.Robot)
                {
                    rowString += '@';
                }
                else if (c == WarehouseObject.BoxLeft)
                {
                    rowString += '[';
                }
                else if (c == WarehouseObject.BoxRight)
                {
                    rowString += ']';
                }
                else if (c == WarehouseObject.Box)
                {
                    rowString += 'O';
                }
                else
                {
                    throw new InvalidOperationException($"Invalid warehouse object {c} at position {x}, {y}");
                }
            }
            inputData.Add(rowString);
        }

        return ParseWarehouseObjects(inputData);
    }

    private WarehouseObject ParseWarehouseObject(char value)
    {
        var d = new Dictionary<char, WarehouseObject>()
        {
            {'.', WarehouseObject.Empty},
            {'#', WarehouseObject.Wall},
            {'O', WarehouseObject.Box},
            {'@', WarehouseObject.Robot},
            {'[', WarehouseObject.BoxLeft},
            {']', WarehouseObject.BoxRight}
        };
        if (d.TryGetValue(value, out var x))
            return x;
        return WarehouseObject.Empty;
    }

    public enum WarehouseObject
    {
        Empty,
        Wall,
        Box,
        Robot,
        BoxLeft,
        BoxRight
    }

    public Direction[] ParseDirection(char[] data)
    {
        var result = new Direction[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = GetDirection(data[i]);
        }
        return result.Where(e => e != Direction.Unknown).ToArray();
    }

    public Direction GetDirection(char value)
    {
        var d = new Dictionary<char, Direction>()
        {
            { '^', Direction.Up },
            { 'v', Direction.Down },
            { 'V', Direction.Down },
            { '<', Direction.Left },
            { '>', Direction.Right }
        };
        if (d.TryGetValue(value, out var x))
            return x;
        return Direction.Unknown;
    }
    public static ReadOnlyDictionary<Direction, Point> DirectionOffset => new Dictionary<Direction, Point>()
    {
        {Direction.Up, new(0, -1)},
        {Direction.Down, new(0, 1)},
        {Direction.Left, new(-1, 0)},
        {Direction.Right, new(1, 0)},
    }.AsReadOnly();
    public enum Direction
    {
        Unknown = -1,
        Up = 0,
        Down,
        Left,
        Right
    }
}