/*
 * MIT License
 * 
 * Copyright (c) 2022-2025 Kate Ward <kate@dariox.club>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using AdventOfCode.TwentyTwentyFive;

namespace AdventOfCode;

public static class Extensions2025
{
    public static T Add2025Types<T>(this T types)
        where T : ICollection<Type>
    {
        types.Add(typeof(Day1));
        types.Add(typeof(Day2));
        types.Add(typeof(Day3));
        types.Add(typeof(Day4));
        types.Add(typeof(Day5));
        types.Add(typeof(Day6));
        types.Add(typeof(Day7));
        types.Add(typeof(Day8));
        types.Add(typeof(Day9));
        types.Add(typeof(Day10));
        types.Add(typeof(Day11));
        types.Add(typeof(Day12));
        return types;
    }
}