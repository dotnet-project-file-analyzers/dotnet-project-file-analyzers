using DotNetProjectFile.TextSimilarity;

namespace System;

internal static class StringExtensions
{
    public static bool Contains(this string? str, string value, StringComparison comparisonType)
        => str is { } && str.IndexOf(value, comparisonType) != -1;

    /// <summary>Matches both strings ignoring casting.</summary>
    public static bool IsMatch(this string? str, string? other)
        => string.Equals(str, other, StringComparison.OrdinalIgnoreCase);

    public static string TrimStart(this string str, string? other)
    {
        if (other is not { Length: > 0 })
        {
            return str;
        }

        var result = str;
        
        while (result.StartsWith(other))
        {
            result = result.Substring(other.Length);
        }

        return result;
    }

    public static string TrimEnd(this string str, string? other)
    {
        if (other is not { Length: > 0 })
        {
            return str;
        }

        var result = str;

        while (result.EndsWith(other))
        {
            result = result.Substring(0, str.Length - other.Length);
        }

        return result;
    }

    /// <inheritdoc cref="NGramsCollection.Create(string?, int)" />
    public static NGramsCollection GetNGrams(this string? str, int n)
        => NGramsCollection.Create(str, n);
}
