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

/// <summary>
/// Quick 2D Point (for <see cref="double"/> precision)
/// </summary>
public readonly struct QDPoint(double x, double y)
    : IEqualityOperators<QDPoint, QDPoint, bool>
    , IEqualityOperators<QDPoint, QPoint, bool>
    , IEquatable<QDPoint>

    , IAdditionOperators<QDPoint, QDPoint, QDPoint>
    , ISubtractionOperators<QDPoint, QDPoint, QDPoint>
    , IAdditionOperators<QDPoint, QPoint, QDPoint>
    , ISubtractionOperators<QDPoint, QPoint, QDPoint>
    
    , IAdditionOperators<QDPoint, float, QDPoint>
    , IAdditionOperators<QDPoint, double, QDPoint>
    , ISubtractionOperators<QDPoint, float, QDPoint>
    , ISubtractionOperators<QDPoint, double, QDPoint>
{
    public double X {get;} = x;
    public double Y {get;} = y;
    public override string ToString()
    {
        return $"<{X},{Y}>";
    }
    private const string TypeIdent = nameof(QDPoint);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode());
    }

    private static bool ValueEquals(QDPoint? left, QDPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }
    private static bool ValueEquals(QDPoint? left, QPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }

    #region Interface Implementations
    public static bool operator ==(QDPoint left, QDPoint right) =>  ValueEquals(left, right);
    public static bool operator !=(QDPoint left, QDPoint right) => !ValueEquals(left, right);
    public static bool operator ==(QDPoint left, QPoint right)  =>  ValueEquals(left, right);
    public static bool operator !=(QDPoint left, QPoint right)  => !ValueEquals(left, right);
    
    public static QDPoint operator +(QDPoint self, QDPoint right) => new(self.X + right.X, self.Y + right.Y);
    public static QDPoint operator -(QDPoint self, QDPoint other) => new(self.X - other.X, self.Y - other.Y);
    public static QDPoint operator +(QDPoint self, QPoint right)  => new(self.X + right.X, self.Y + right.Y);
    public static QDPoint operator -(QDPoint self, QPoint other)  => new(self.X - other.X, self.Y - other.Y);
    
    public static QDPoint operator +(QDPoint self, float right)  => new(self.X + right, self.Y + right);
    public static QDPoint operator +(QDPoint self, double right) => new(self.X + right, self.Y + right);
    public static QDPoint operator -(QDPoint self, float right)  => new(self.X - right, self.Y - right);
    public static QDPoint operator -(QDPoint self, double right) => new(self.X - right, self.Y - right);
    public bool Equals(QDPoint other) => ValueEquals(this, other);
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QDPoint point) return ValueEquals(this, point);
        return false;
    }
    #endregion
    
    public static implicit operator QDPoint(QPoint point) => new(point.X, point.Y);
}