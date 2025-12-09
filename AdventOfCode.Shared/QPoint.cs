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
/// Quick Point
/// </summary>
public readonly struct QPoint(int x, int y)
    : IEqualityOperators<QPoint, QPoint, bool>
    , IEqualityOperators<QPoint, QLPoint, bool>
    , IEquatable<QPoint>
    
    , IAdditionOperators<QPoint, QPoint, QPoint>
    , ISubtractionOperators<QPoint, QPoint, QPoint>

    , IAdditionOperators<QPoint, int, QPoint>
    , ISubtractionOperators<QPoint, int, QPoint>
    
    , IAdditionOperators<QPoint, float, QDPoint>
    , IAdditionOperators<QPoint, double, QDPoint>
    , ISubtractionOperators<QPoint, float, QDPoint>
    , ISubtractionOperators<QPoint, double, QDPoint>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public override string ToString()
    {
        return $"<{X},{Y}>";
    }
    private const string TypeIdent = nameof(QRectangle);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode());
    }
    
    private static bool ValueEquals(QPoint? left, QPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }
    private static bool ValueEquals(QPoint? left, QLPoint? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y;
    }

    #region Interface Implementations
    public static bool operator ==(QPoint left, QPoint right) => ValueEquals(left, right);
    public static bool operator !=(QPoint left, QPoint right) => !ValueEquals(left, right);
    public static bool operator ==(QPoint left, QLPoint right) => ValueEquals(left, right);
    public static bool operator !=(QPoint left, QLPoint right) => !ValueEquals(left, right);

    public static QPoint operator +(QPoint left, QPoint right) => new(left.X + right.X, left.Y + right.Y);
    public static QPoint operator -(QPoint left, QPoint right) => new(left.X - right.X, left.Y - right.Y);
    
    public static QPoint operator +(QPoint self, int other)    => new(self.X + other, self.Y + other);
    public static QPoint operator -(QPoint self, int other)    => new(self.X - other, self.Y - other);
    
    
    public static QDPoint operator +(QPoint self, float right)  => new(self.X + right, self.Y + right);
    public static QDPoint operator +(QPoint self, double right) => new(self.X + right, self.Y + right);
    public static QDPoint operator -(QPoint self, float right)  => new(self.X - right, self.Y - right);
    public static QDPoint operator -(QPoint self, double right) => new(self.X - right, self.Y - right);

    public bool Equals(QPoint other) => ValueEquals(this, other);
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QPoint point) return ValueEquals(this, point);
        return false;
    }
    #endregion

    public static explicit operator QPoint(QDPoint point)
        => new(Convert.ToInt32(Math.Floor(point.X)),
               Convert.ToInt32(Math.Floor(point.Y)));
}