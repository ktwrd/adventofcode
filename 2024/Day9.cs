using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(9)]
public class DayNine : IDayHandler
{
	public void Run(string[] lines)
	{
		var a = PartOne(string.Join("", lines).ToCharArray());
		Console.WriteLine($"Part One: {a}");
		var b = PartTwo(string.Join("", lines).ToCharArray());
		Console.WriteLine($"Part Two: {b}");
	}

	private long PartOne(char[] inputData)
	{
		var expanded = ExpandDiskMap(inputData);
		var moved = MoveBlocks(expanded);
		return CalculateChecksum(moved);
	}

	private long PartTwo(char[] inputData)
	{
		var expanded = ExpandDiskMap(inputData);
		var files = CompactDiskPartTwo(expanded);
		return CalculateChecksum(files);
	}

	public record struct Block()
	{
		public int Id { get; set; } = 0;
		public int Start { get; set; } = 0;
		public int Length { get; set; } = 0;
	}

	public static int[] ExpandDiskMap(char[] inputData)
	{
		var blocks = new List<int>();
		int index = 0;
		for (int i = 0; i < inputData.Length; i++)
		{
			var current = int.Parse(inputData[i].ToString());

			if (i % 2 == 0)
			{
				if (current != 0)
				{
					for (int c = 0; c < current; c++)
					{
						blocks.Add(index);
					}
				}
				index++;
			}
			else
			{
				if (current != 0)
				{
					for (int c = 0; c < current; c++)
					{
						blocks.Add(-1);
					}
				}
			}
		}

		return blocks.ToArray();
	}

	public static List<Block> FindFiles(int[] inputData)
	{
		var result = new Dictionary<string, Block>();
		for (int i = 0; i < inputData.Length; i++)
		{
			var id = inputData[i].ToString();

			if (!result.ContainsKey(id))
			{
				result[id] = new Block()
				{
					Id = inputData[i],
					Start = i,
					Length = 1
				};
			}
			else
			{
				var p = result[id];
				p.Length++;
				result[id] = p;
			}
		}

		return result.Select(e => e.Value).ToList();
	}

	public static int FindFreeSpace(int[] inputData, int start, int length)
	{
		var data = new List<int>(inputData);
		int currentLength = 0;
		int currentStart = -1;
		for (int i = 0; i < start; i++)
		{
			if (data[i] == -1)
			{
				if (currentStart == -1)
				{
					currentStart = i;
				}

				currentLength++;

				if (currentLength == length)
				{
					return currentStart;
				}
			}
			else
			{
				currentLength = 0;
				currentStart = -1;
			}
		}

		return -1;
	}

	public static int[] MoveFile(int[] data, Block file, int newStart)
	{
		var result = new List<int>(data);
		for (int i = file.Start; i < file.Start + file.Length; i++)
		{
			result[i] = -1;
		}

		for (int i = 0; i < file.Length; i++)
		{
			result[i + newStart] = file.Id;
		}

		return result.ToArray();
	}

	public static int[] CompactDiskPartTwo(int[] inputData)
	{
		var files = FindFiles(inputData).OrderByDescending(x => x.Id).ToList();

		var data = inputData;
		foreach (var f in files)
		{
			var newPosition = FindFreeSpace(data, f.Start, f.Length);
			if (newPosition != -1)
			{
				data = MoveFile(data, f, newPosition);
			}
		}

		return data;
	}

	private int[] MoveBlocks(int[] data)
	{
		var result = new List<int>(data);


		while (true)
		{
			var gap = result.IndexOf(-1);
			if (gap == -1) break;

			var lastIndex = result.Count - 1;
			while (lastIndex >= 0 && result[lastIndex] == -1)
			{
				lastIndex -= 1;
			}

			if (lastIndex <= gap)
				break;

			result[gap] = result[lastIndex];
			result[lastIndex] = -1;
		}

		return result.ToArray();
	}
	
	private long CalculateChecksum(int[] data)
	{
		long total = 0;
		for (long i = 0; i < data.Length; i++)
		{
			if (data[i] != -1)
			{
				total += i * data[i];
			}
		}

		return total;
	}
}