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
namespace AdventOfCode;

/// <summary>
/// Quick Edge using <see cref="QPoint"/>
/// </summary>
public readonly struct QEdge(QPoint a, QPoint b)
{
    public bool IsHorizontal { get; } = a.Y == b.Y;
    public QPoint Start { get; } = a.Y == b.Y ? (a.X < b.X ? a : b) : (a.Y < b.Y ? a : b);
    public QPoint End { get; } = a.Y == b.Y ? (a.X < b.X ? b : a) : (a.Y < b.Y ? b : a);

    public bool Intersects(QEdge other)
    {
        if (IsHorizontal == other.IsHorizontal) return false;

        var h = IsHorizontal ? this : other;
        var v = IsHorizontal ? other : this;

        return v.Start.X > h.Start.X && v.Start.X < h.End.X
            && h.Start.Y > v.Start.Y && h.Start.Y < v.End.Y;
    }

    private const string TypeIdent = nameof(QEdge);
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeIdent.GetHashCode(), Start.GetHashCode(), End.GetHashCode());
    }
    public override string ToString() => $"<Start={Start},End={End}>";
}