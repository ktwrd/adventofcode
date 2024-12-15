using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            if (robotPosition.X == -1 && robotPosition.Y == -1)
            {
                throw new InvalidDataException($"Could not find robot in warehouse!");
            }

            var (moveKind, freeSpace, offset) = CanRobotMove(robotPosition, move, warehouse);
            if (moveKind == RobotMoveKind.MoveRobot)
            {
                warehouse[robotPosition.X, robotPosition.Y] = WarehouseObject.Empty;
                robotPosition.X += offset.X;
                robotPosition.Y += offset.Y;
                warehouse[robotPosition.X, robotPosition.Y] = WarehouseObject.Robot;
            }
            else if (moveKind == RobotMoveKind.MoveRobotAndBox && freeSpace != null)
            {
                warehouse[robotPosition.X, robotPosition.Y] = WarehouseObject.Empty;
                robotPosition.X += offset.X;
                robotPosition.Y += offset.Y;
                warehouse[robotPosition.X, robotPosition.Y] = WarehouseObject.Robot;
                warehouse[freeSpace.Value.X, freeSpace.Value.Y] = WarehouseObject.Box;
            }
        }
        PrintGrid(warehouse);
        double result = 0;

        for (int x = 0; x < warehouse.GetLength(0); x++)
        {
            for (int y = 0; y < warehouse.GetLength(1); y++)
            {
                var w = warehouse[x, y];
                if (w == WarehouseObject.Box)
                {
                    result += (100 * y) + x;
                }
            }
        }
        return Convert.ToInt64(Math.Round(result));
    }
    public long PartTwo(string[] inputData)
    {
        long result = 0;
        return result;
    }

    public void PrintGrid(WarehouseObject[,] grid)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                var g = grid[x, y];
                var c = '.';
                if (g == WarehouseObject.Box)
                {
                    c = 'O';
                }
                else if (g == WarehouseObject.Robot)
                {
                    c = '@';
                }
                else if (g == WarehouseObject.Wall)
                {
                    c = '#';
                }
                else
                {
                    c = '.';
                }
                Console.Write(c);
            }
            Console.Write(Environment.NewLine);
        }
    }

    public (RobotMoveKind kind, Point? freeSpace, Point offset) CanRobotMove(Point robotPosition, Direction direction, WarehouseObject[,] warehouse)
    {
        var width = warehouse.GetLength(0);
        var height = warehouse.GetLength(1);
        var offset = DirectionOffset[direction];
        var newPosition = new Point(robotPosition.X + offset.X, robotPosition.Y + offset.Y);
        if (newPosition.X >= width || newPosition.X < 0 || newPosition.Y >= height || newPosition.Y < 0)
            return (RobotMoveKind.InvalidPosition, null, offset);
        if (warehouse[newPosition.X, newPosition.Y] == WarehouseObject.Wall)
            return (RobotMoveKind.Nothing, null, offset);
        if (warehouse[newPosition.X, newPosition.Y] == WarehouseObject.Box)
        {
            var checkQueue = new Queue<Point>();
            checkQueue.Enqueue(newPosition);
            while (checkQueue.Count > 0)
            {
                var pop = checkQueue.Dequeue();
                if (pop.X < 0 || pop.X >= width || pop.Y < 0 || pop.Y >= height)
                {
                    throw new InvalidOperationException($"Queue item {pop} is out of bounds");
                }

                if (warehouse[pop.X, pop.Y] == WarehouseObject.Box)
                {
                    checkQueue.Enqueue(new(pop.X + offset.X, pop.Y + offset.Y));
                }
                else if (warehouse[pop.X, pop.Y] == WarehouseObject.Wall)
                {
                    return (RobotMoveKind.Nothing, null, offset);
                }
                else
                {
                    return (RobotMoveKind.MoveRobotAndBox, new(pop.X, pop.Y), offset);
                }
            }
        }
        return (RobotMoveKind.MoveRobot, newPosition, offset);
    }

    public enum RobotMoveKind
    {
        InvalidPosition,
        Nothing,
        MoveRobot,
        MoveRobotAndBox
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

    public (WarehouseObject[,] warehouse, Direction[] robotMovements) Parse(IList<string> inputData)
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
                if (row.Length < warehouseWidth)
                    continue;
                for (int x = 0; x < warehouseWidth; x++)
                {
                    warehouse[x, y] = ParseWarehouseObject(row[x]);
                }
            }
        }

        var robotMovements = ParseDirection(robotMovementString.Trim(' ').ToCharArray());
        return (warehouse, robotMovements);
    }

    private WarehouseObject ParseWarehouseObject(char value)
    {
        var d = new Dictionary<char, WarehouseObject>()
        {
            {'.', WarehouseObject.Empty},
            {'#', WarehouseObject.Wall},
            {'O', WarehouseObject.Box},
            {'@', WarehouseObject.Robot},
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
        Robot
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