namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class AdventAttribute(int year, int day) : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
}