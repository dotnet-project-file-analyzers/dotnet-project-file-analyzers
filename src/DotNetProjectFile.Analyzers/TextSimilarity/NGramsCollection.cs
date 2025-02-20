using System.Collections.Frozen;

namespace DotNetProjectFile.TextSimilarity;

public sealed class NGramsCollection
{
    private NGramsCollection(int n, FrozenDictionary<string, int> entries, int count)
    {
        N = n;
        Count = count;
        Entries = entries;
    }

    public int N { get; }

    public int Count { get; }

    public FrozenDictionary<string, int> Entries { get; }

    /// <summary>
    /// Gets the occurrence count of the given <paramref name="ngram"/>.
    /// </summary>
    /// <param name="ngram">The n-gram to search for.</param>
    /// <returns>The number of occurrences.</returns>
    public int GetCount(string ngram)
        => Entries.TryGetValue(ngram, out var cnt)
        ? cnt
        : 0;

    /// <summary>
    /// Creates a new n-gram collection.
    /// </summary>
    /// <param name="str">The string to create the collection for.</param>
    /// <param name="n">The size of the n-gram.</param>
    /// <returns>The newly created collection.</returns>
    public static NGramsCollection Create(string? str, int n)
    {
        str ??= string.Empty;

        var result = new Dictionary<string, int>();
        var size = 0;

        for (var i = n; i < str.Length; i++)
        {
            var qgram = str.Substring(i - n, n);

            if (!result.TryGetValue(qgram, out var count))
            {
                count = 0;
            }

            result[qgram] = count + 1;
            size++;
        }

        return new(n, result.ToFrozenDictionary(), size);
    }
}

public static class NGramsCollectionExtensions
{
    /// <inheritdoc cref="DotNetProjectFile.TextSimilarity.JaccardIndex.Compute(NGramsCollection, NGramsCollection)" />
    public static float JaccardIndex(this NGramsCollection ngrams1, NGramsCollection ngrams2)
        => DotNetProjectFile.TextSimilarity.JaccardIndex.Compute(ngrams1, ngrams2);

    /// <inheritdoc cref="DotNetProjectFile.TextSimilarity.DiceSorensenCoefficient.Compute(NGramsCollection, NGramsCollection)" />
    public static float DiceSorensenCoefficient(this NGramsCollection ngrams1, NGramsCollection ngrams2)
        => DotNetProjectFile.TextSimilarity.DiceSorensenCoefficient.Compute(ngrams1, ngrams2);
}
