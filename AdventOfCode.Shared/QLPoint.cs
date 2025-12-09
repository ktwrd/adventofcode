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

public readonly struct QLPoint(long x, long y)
    : IEqualityOperators<QLPoint, QLPoint, bool>
    , IEqualityOperators<QLPoint, QPoint, bool>
    , IEquatable<QLPoint>
    
    , IAdditionOperators<QLPoint, QLPoint, QLPoint>
    , IAdditionOperators<QLPoint, QPoint, QLPoint>
    , ISubtractionOperators<QLPoint, QLPoint, QLPoint>
    , ISubtractionOperators<QLPoint, QPoint, QLPoint>

    , IAdditionOperators<QLPoint, int, QLPoint>
    , IAdditionOperators<QLPoint, long, QLPoint>
    , ISubtractionOperators<QLPoint, int, QLPoint>
    , ISubtractionOperators<QLPoint, long, QLPoint>
{
    public long X { get; } = x;
    public long Y { get; } = y;
    public override string ToString()
    {
        return $"<{X},{Y}>";
    }
    private const string TypeIdent = nameof(QLPoint);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode());
    }
    
    private static bool ValueEquals(QLPoint? left, QLPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }
    private static bool ValueEquals(QLPoint? left, QPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }

    #region Interface Implementations
    public static bool operator ==(QLPoint left, QLPoint right) =>  ValueEquals(left, right);
    public static bool operator !=(QLPoint left, QLPoint right) => !ValueEquals(left, right);
    public static bool operator ==(QLPoint left, QPoint right)  =>  ValueEquals(left, right);
    public static bool operator !=(QLPoint left, QPoint right)  => !ValueEquals(left, right);

    public static QLPoint operator +(QLPoint self, QLPoint right) => new(self.X + right.X, self.Y + right.Y);
    public static QLPoint operator +(QLPoint self, QPoint other)  => new(self.X + other.X, self.Y + other.Y);
    public static QLPoint operator -(QLPoint self, QLPoint other) => new(self.X - other.X, self.Y - other.Y);
    public static QLPoint operator -(QLPoint self, QPoint other)  => new(self.X - other.X, self.Y - other.Y);

    public static QLPoint operator +(QLPoint self, int other)     => new(self.X + other, self.Y + other);
    public static QLPoint operator +(QLPoint self, long other)    => new(self.X + other, self.Y + other);
    public static QLPoint operator -(QLPoint self, int other)     => new(self.X - other, self.Y - other);
    public static QLPoint operator -(QLPoint self, long other)    => new(self.X - other, self.Y - other);
    public bool Equals(QLPoint other) => ValueEquals(this, other);
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QLPoint point) return ValueEquals(this, point);
        return false;
    }
    #endregion
    
    public static explicit operator QLPoint(QPoint point) => new(point.X, point.Y);
}