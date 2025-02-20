namespace DotNetProjectFile.TextSimilarity;

public static class JaccardIndex
{
    /// <summary>
    /// Computes the Jaccard index between two n-grams collections.
    /// </summary>
    /// <param name="ngrams1">The first n-gram collection.</param>
    /// <param name="ngrams2">The second n-gram collection.</param>
    /// <returns>The found Jaccard index.</returns>
    /// <seealso href="https://en.wikipedia.org/wiki/Jaccard_index"/>
    public static float Compute(NGramsCollection ngrams1, NGramsCollection ngrams2)
    {
        var intersection = 0;

        foreach (var pair in ngrams1.Entries)
        {
            var countInA = pair.Value;
            var countInB = ngrams2.GetCount(pair.Key);

            var countInBoth = Math.Min(countInA, countInB);
            intersection += countInBoth;
        }

        var union = ngrams1.Count + ngrams2.Count - intersection;
        var index = (float)intersection / union;
        return index;
    }
}
