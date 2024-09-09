namespace System;

internal static class StringExtensions
{
    public static bool Contains(this string? str, string value, StringComparison comparisonType)
        => str is { } && str.IndexOf(value, comparisonType) != -1;

    /// <summary>Matches both strings ignoring casting.</summary>
    public static bool IsMatch(this string? str, string? other)
        => string.Equals(str, other, StringComparison.OrdinalIgnoreCase);
}
