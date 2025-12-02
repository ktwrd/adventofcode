using System.ComponentModel;

namespace AdventOfCode.TwentyTwentyFour;

[DefaultValue(17)]
public class DaySeventeen : IDayHandler
{
	public void Run(string[] inputData)
	{
		var a = PartOne(inputData);
		Console.WriteLine($"Part One: {a}");
		var b = PartTwo(inputData);
		Console.WriteLine($"Part Two: {b}");
	}

	public string PartOne(string[] lines)
	{
		var data = ParseInput(lines);

		return Process(data);
	}

	public string Process(Computer data, bool partTwo = false)
	{
		
		/*
		Console.WriteLine("Output: " + string.Join(',', output));
		Console.WriteLine("A " + a);
		Console.WriteLine("B " + b);
		Console.WriteLine("C " + c);
		Console.WriteLine("Program: " + string.Join(',', data.Program));
		*/
		var output = ProcessRaw(data);
		return string.Join("", output);
	}

	public List<long> ProcessRaw(Computer data)
	{
		var a = data.Register.A;
		var b = data.Register.B;
		var c = data.Register.C;

		var output = new List<long>();

		long ParseOperand(long value)
		{
			if (value <= 3 && value >= 0)
				return value;
			if (value == 4)
				return a;
			if (value == 5)
				return b;
			if (value == 6)
				return c;
			return -1;
		}

		int RunSingular(long opcode, long operand)
		{
			var combo = ParseOperand(operand);

			if (opcode == 0)
			{
				a = Convert.ToInt64(Math.Floor(a / Math.Pow(2, combo)));
			}
			else if (opcode == 1)
			{
				var x = b ^ operand;
				b = x;
			}
			else if (opcode == 2)
			{
				b = combo % 8;
			}
			else if (opcode == 3)
			{
				if (a != 0)
				{
					var op = Convert.ToInt32(operand % data.Program.Length);
					return op;
				}
			}
			else if (opcode == 4)
			{
				var x = b ^ c;
				b = x;
			}
			else if (opcode == 5)
			{
				output.Add(combo % 8);
			}
			else if (opcode == 6)
			{
				b = Convert.ToInt64(Math.Floor(a / Math.Pow(2, combo)));
			}
			else if (opcode == 7)
			{
				c = Convert.ToInt64(Math.Floor(a / Math.Pow(2, combo)));
			}

			return -1;
		}

		void RunProgram(int startIndex)
		{
			int i = 0;
			while (i < data.Program.Length)
			{
				bool increment = true;
				if (i % 2 == 0)
				{
					if (i + 1 >= data.Program.Length)
					{
						return;
					}
					var opcode = data.Program[i];
					var operand = data.Program[i + 1];
					var r = RunSingular(opcode, operand);
					if (r != -1)
					{
						i = r;
						increment = false;
					}
				}

				if (increment)
				{
					i++;
				}
			}
		}
		
		RunProgram(0);
		return output;
	}
	public long PartTwo(string[] lines)
	{
		var data = ParseInput(lines);
		return PartTwoSolve(0, data.Program.Length - 1, data);
	}

	public long PartTwoSolve(long nextValue, int index, Computer computer)
	{
		if (index < 0)
			return nextValue;
		for (var aval = nextValue * 8; aval < nextValue * 8 + 8; aval++)
		{
			computer.Register.A = aval;
			var output = ProcessRaw(computer);
			if (output[0] == computer.Program[index])
			{
				var finalValue = PartTwoSolve(aval, index - 1, computer);
				if (finalValue >= 0)
				{
					return finalValue;
				}
			}
		}

		return -1;
	}
	public enum Opcode
	{
		/// <summary>
		/// Numerator is in Register A, denominator is the combo operand to the power of 2.
		///
		/// The result is truncated to an integer then written to the A register.
		/// </summary>
		Division = 0,
		/// <summary>
		/// Left hand is Register B, and the right hand is the literal operand.
		/// </summary>
		BitwiseXOR = 1,
		/// <summary>
		/// Left hand is the combo operand, right hand is <c>8</c>
		/// </summary>
		Modulo = 2,
		/// <summary>
		/// Jump to program index at Register A when it's not zero.
		/// </summary>
		JumpIfNotZero = 3,
		/// <summary>
		/// Left hand is Register B, Right Hand is Register C (read operand, but ignore)
		/// </summary>
		BitwiseXorAlt = 4,
		/// <summary>
		/// Write to the output modulus. Where left hand is the combo, and the right hand is <c>8</c>
		/// </summary>
		Out = 5,
		/// <summary>
		/// 
		/// </summary>
		DivisionAlt = 6,
	}
	public enum Operand
	{
		LiteralZero = 0,
		LiteralOne = 1,
		LiteralTwo = 2,
		LiteralThree = 3,
		RegisterAValue = 4,
		RegisterBValue = 5,
		RegisterCValue = 6
	}

	public Computer ParseInput(string[] lines)
	{
		var result = new Computer();
		foreach (var line in lines)
		{
			if (line.StartsWith("Register A:"))
			{
				result.Register.A = long.Parse(line.Split(":")[1].Trim());
			}
			else if (line.StartsWith("Register B:"))
			{
				result.Register.B = long.Parse(line.Split(":")[1].Trim());
			}
			else if (line.StartsWith("Register C:"))
			{
				result.Register.C = long.Parse(line.Split(":")[1].Trim());
			}
			else if (line.StartsWith("Program:"))
			{
				var data = line.Split(":")[1].Trim().Split(",");
				result.Program = data.Select(long.Parse).ToArray();
			}
		}
		return result;
	}

	public struct Computer
	{
		public ComputerRegister Register;

		public long[] Program;
	}

	public struct ComputerRegister
	{
		public long A;
		public long B;
		public long C;
	}
}
