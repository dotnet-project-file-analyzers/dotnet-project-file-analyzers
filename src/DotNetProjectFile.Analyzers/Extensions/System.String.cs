namespace System;

internal static class StringExtensions
{
    public static bool Contains(this string? str, string value, StringComparison comparisonType)
        => str is { } && str.IndexOf(value, comparisonType) != -1;
}
