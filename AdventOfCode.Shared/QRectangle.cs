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

namespace AdventOfCode;

public readonly struct QRectangle
{
    public QRectangle(int x, int y, int w, int h)
    {
        X = x;
        Y = y;
        Width = w;
        Height = h;
        Area = (long)Height * Width;
    }
    public QRectangle(QPoint topLeft, QPoint bottomRight)
    {
        X = Math.Min(topLeft.X, bottomRight.X);
        Y = Math.Min(topLeft.Y, bottomRight.Y);
        Width = Math.Max(topLeft.X, bottomRight.X) - X;
        Height = Math.Max(topLeft.Y, bottomRight.Y) - Y;
        Area = (long)Width * Height;
    }
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public long Area { get; }

    public override string ToString()
    {
        return $"<X={X},Y={Y},W={Width},H={Height}>";
    }
    private const string TypeIdent = nameof(QRectangle);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), X.GetHashCode(), Y.GetHashCode(), Width.GetHashCode(), Height.GetHashCode());
    }

    public QEdge[] GetEdges()
    {
        return [
            new QEdge(new QPoint(X,         Y),          new QPoint(X + Width, Y)),
            new QEdge(new QPoint(X + Width, Y),          new QPoint(X + Width, Y + Height)),
            new QEdge(new QPoint(X + Width, Y + Height), new QPoint(X,         Y + Height)),
            new QEdge(new QPoint(X,         Y + Height), new QPoint(X,         Y)),
        ];
    }
    
    private static bool ValueEquals(QRectangle? left, QRectangle? right)
    {
        return left?.X == right?.X
            && left?.Y == right?.Y
            && left?.Width == right?.Width
            && left?.Height == right?.Height;
    }
    
    #region Interface Implementations
    public static bool operator ==(QRectangle left, QRectangle right) =>  ValueEquals(left, right);
    public static bool operator !=(QRectangle left, QRectangle right) => !ValueEquals(left, right);
    #endregion
    
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is QRectangle rect) return ValueEquals(this, rect);
        return false;
    }
}