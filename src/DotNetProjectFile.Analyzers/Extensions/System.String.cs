using static System.Net.WebRequestMethods;

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
    /// Computes the Dice-Sørensen coefficient between two strings <paramref name="a"/>
    /// and <paramref name="b"/>. Using the given <paramref name="q"/>-grams.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="q">The q-gram size.</param>
    /// <param name="filterDuplicates">Indicates whether or not duplicate q-grams should be ignored.</param>
    /// <returns>
    /// A similarity coefficient in the range
    /// [<see langword="0"/>, <see langword="1"/>].
    /// </returns>
    /// <seealso href="https://en.wikipedia.org/wiki/Dice-S%C3%B8rensen_coefficient"/>
    public static float DiceSorensenCoefficient(this string? a, string? b, int q, bool filterDuplicates)
    {
        var j = JaccardIndex(a, b, q, filterDuplicates);
        var s = (2 * j) / (1 + j);
        return s;
    }

    /// <summary>
    /// Computes the Jaccard index between two strings <paramref name="a"/>
    /// and <paramref name="b"/>. Using the given <paramref name="q"/>-grams.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="q">The q-gram size.</param>
    /// <param name="filterDuplicates">Indicates whether or not duplicate q-grams should be ignored.</param>
    /// <returns>The found Jaccard index.</returns>
    /// <seealso href="https://en.wikipedia.org/wiki/Jaccard_index"/>
    public static float JaccardIndex(this string? a, string? b, int q, bool filterDuplicates)
    {
        a ??= string.Empty;
        b ??= string.Empty;

        var (aGrams, aCount) = GetQGrams(a, q, filterDuplicates);
        var (bGrams, bCount) = GetQGrams(b, q, filterDuplicates);

        var intersection = 0;

        foreach (var pair in aGrams)
        {
            if (bGrams.TryGetValue(pair.Key, out var countInB))
            {
                var countInA = pair.Value;

                var countInBoth = Math.Min(countInA, countInB);
                intersection += countInBoth;
            }
        }

        var union = aCount + bCount - intersection;
        var index = (float)intersection / union;
        return index;
    }

    private static (Dictionary<string, int> Lookup, int Count) GetQGrams(string value, int q, bool filterDuplicates)
    {
        var result = new Dictionary<string, int>();
        var size = 0;

        for (var i = q; i < value.Length; i++)
        {
            var qgram = value.Substring(i - q, q);

            if (!result.TryGetValue(qgram, out var count))
            {
                count = 0;
            }

            result[qgram] = filterDuplicates ? 1 : count + 1;
            size++;
        }

        return (result, size);
    }
}
