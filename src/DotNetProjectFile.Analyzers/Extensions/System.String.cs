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

    /// <summary>
    /// Computes the Damerau-Levenshtein distance between the <paramref name="a"/> and <paramref name="b"/> strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The distance between the strings.</returns>
    /// <remarks>
    /// Implementation of pseudocode provided by link below.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance#Optimal_string_alignment_distance"/>
    public static int DamerauLevenshteinDistanceTo(this string? a, string? b)
    {
        a ??= string.Empty;
        b ??= string.Empty;

        var d = new int[a.Length + 1, b.Length + 1];

        for (var i = 0; i <= a.Length; i++)
        {
            d[i, 0] = i;
        }

        for (var j = 0; j <= b.Length; j++)
        {
            d[0, j] = j;
        }

        for (var i = 1; i <= a.Length; i++)
        {
            for (var j = 1; j <= b.Length; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);

                if (i > 1
                 && j > 1
                 && a[i - 1] == b[j - 2]
                 && a[i - 2] == b[j - 1])
                {
                    d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + 1);
                }
            }
        }

        return d[a.Length, b.Length];
    }
}
