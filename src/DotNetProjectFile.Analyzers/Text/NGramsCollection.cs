namespace DotNetProjectFile.Text;

public sealed class NGramsCollection
{
    /// <summary>Each char takes 16 bits of the <see cref="ulong"/> key.</summary>
    private const int MaxN = 4;

    private NGramsCollection(int n, Dictionary<ulong, int> entries, int count)
    {
        N = n;
        Count = count;
        Entries = entries;
    }

    public int N { get; }

    public int Count { get; }

    /// <summary>The n-grams (packed as <see cref="ulong"/>) and their occurrence counts.</summary>
    public IReadOnlyDictionary<ulong, int> Entries { get; }

    /// <summary>
    /// Gets the occurrence count of the given (packed) <paramref name="ngram"/>.
    /// </summary>
    /// <param name="ngram">The n-gram to search for.</param>
    /// <returns>The number of occurrences.</returns>
    public int GetCount(ulong ngram)
        => Entries.TryGetValue(ngram, out var cnt)
        ? cnt
        : 0;

    /// <summary>
    /// Creates a new n-gram collection.
    /// </summary>
    /// <param name="str">The string to create the collection for.</param>
    /// <param name="n">The size of the n-gram (1 to 4).</param>
    /// <returns>The newly created collection.</returns>
    public static NGramsCollection Create(string? str, int n)
    {
        if (n is < 1 or > MaxN) throw new ArgumentOutOfRangeException(nameof(n), n, $"n must be between 1 and {MaxN}.");

        str ??= string.Empty;

        var result = new Dictionary<ulong, int>();
        var size = 0;

        for (var i = n; i <= str.Length; i++)
        {
            var key = 0UL;

            for (var j = i - n; j < i; j++)
            {
                key = (key << 16) | str[j];
            }

            result.TryGetValue(key, out var count);
            result[key] = count + 1;
            size++;
        }

        return new(n, result, size);
    }
}

public static class NGramsCollectionExtensions
{
    /// <inheritdoc cref="JaccardIndex.Compute(NGramsCollection, NGramsCollection)" />
    public static float JaccardIndex(this NGramsCollection ngrams1, NGramsCollection ngrams2)
        => Text.JaccardIndex.Compute(ngrams1, ngrams2);

    /// <inheritdoc cref="DiceSorensenCoefficient.Compute(NGramsCollection, NGramsCollection)" />
    public static float DiceSorensenCoefficient(this NGramsCollection ngrams1, NGramsCollection ngrams2)
        => Text.DiceSorensenCoefficient.Compute(ngrams1, ngrams2);

    /// <inheritdoc cref="DiceSorensenCoefficient.AtLeast(NGramsCollection, NGramsCollection, float)" />
    public static bool DiceSorensenCoefficientAtLeast(this NGramsCollection ngrams1, NGramsCollection ngrams2, float requiredSimilarity)
        => Text.DiceSorensenCoefficient.AtLeast(ngrams1, ngrams2, requiredSimilarity);
}
