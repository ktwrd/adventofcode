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
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode;

public readonly struct QVector3(int x, int y, int z)
    : IEqualityOperators<QVector3, QVector3, bool>
    , IEquatable<QVector3>
    , IAdditionOperators<QVector3, QVector3, QVector3>
    , ISubtractionOperators<QVector3, QVector3, QVector3>
    , IMultiplyOperators<QVector3, QVector3, QVector3>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public override string ToString()
    {
        return $"<{X},{Y},{Z}>";
    }

    private const string TypeIdent = nameof(QVector3);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
    }

    private static bool ValueEquals(QVector3? left, QVector3? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y
            && left?.Z == right?.Z;
    }

    #region Interface Implementations
    public static bool operator ==(QVector3 left, QVector3 right) =>  ValueEquals(left, right);
    public static bool operator !=(QVector3 left, QVector3 right) => !ValueEquals(left, right);
    public static QVector3 operator +(QVector3 left, QVector3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static QVector3 operator -(QVector3 left, QVector3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    public static QVector3 operator *(QVector3 left, QVector3 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    public bool Equals(QVector3 other) => ValueEquals(this, other);
    #endregion

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QVector3 other) return ValueEquals(this, other);
        return false;
    }

    public static QVector3 Parse(string s) => Parse(s, ',');
    public static QVector3 Parse(string s, char separator)
    {
        var numbers = s.Split(separator).Select(int.Parse).ToArray();
        return new QVector3(numbers[0], numbers[1], numbers[2]);
    }
    
    public static bool TryParse(string? s, out QVector3 result) => TryParse(s, ',', out result);
    public static bool TryParse(string? s, char separator, out QVector3 result)
    {
        var numbers = s?.Split(separator).Select(int.Parse).ToArray() ?? [];
        if (numbers.Length < 3)
        {
            result = new(0,0,0);
            return false;
        }
    
        result = new QVector3(numbers[0], numbers[1], numbers[2]);
        return true;
    }
}