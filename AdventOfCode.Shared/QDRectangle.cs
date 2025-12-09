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
/// Quick Rectangle (for <see cref="double"/> precision)
/// </summary>
public readonly struct QDRectangle
    : IEqualityOperators<QDRectangle, QDRectangle, bool>
{
    public QDRectangle(QDPoint topLeft, QDPoint bottomRight)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        Area = (decimal)Width * (decimal)Height;
    }
    public QDPoint TopLeft { get; }
    public QDPoint BottomRight { get; }
    public double X => TopLeft.X;
    public double Y => TopLeft.Y;
    public double Width => BottomRight.X - X;
    public double Height => BottomRight.Y - Y;
    public decimal Area { get; }

    public override string ToString()
    {
        return $"<X={X},Y={Y},W={Width},H={Height}>";
    }
    private const string TypeIdent = nameof(QDRectangle);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode(), Width.GetHashCode(), Height.GetHashCode());
    }

    public QDEdge[] GetEdges()
    {
        return [
            new(new(X,         Y),          new(X + Width, Y)),
            new(new(X + Width, Y),          new(X + Width, Y + Height)),
            new(new(X + Width, Y + Height), new(X,         Y + Height)),
            new(new(X,         Y + Height), new(X,         Y)),
        ];
    }

    private static bool ValueEquals(QDRectangle? left, QDRectangle? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y
            && left?.Width == right?.Width
            && left?.Height == right?.Height;
    }

    #region Interface Implementations
    public static bool operator ==(QDRectangle left, QDRectangle right) =>  ValueEquals(left, right);
    public static bool operator !=(QDRectangle left, QDRectangle right) => !ValueEquals(left, right);
    #endregion

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QDRectangle rect) return ValueEquals(this, rect);
        return false;
    }
}