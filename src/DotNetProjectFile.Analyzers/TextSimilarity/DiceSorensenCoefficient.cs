namespace DotNetProjectFile.TextSimilarity;

public static class DiceSorensenCoefficient
{
    /// <summary>
    /// Computes the Dice-Sørensen coefficient between two n-grams collections.
    /// </summary>
    /// <param name="ngrams1">The first n-gram collection.</param>
    /// <param name="ngrams2">The second n-gram collection.</param>
    /// <returns>
    /// A similarity coefficient in the range
    /// [<see langword="0"/>, <see langword="1"/>].
    /// </returns>
    /// <seealso href="https://en.wikipedia.org/wiki/Dice-S%C3%B8rensen_coefficient"/>
    public static float Compute(NGramsCollection ngrams1, NGramsCollection ngrams2)
    {
        var j = JaccardIndex.Compute(ngrams1, ngrams2);
        var s = (2 * j) / (1 + j);
        return s;
    }

    /// <summary>
    /// Validates that the Dice-Sørensen coefficient between two n-grams collections
    /// is at least equal to the <paramref name="requiredSimilarity"/>.
    /// </summary>
    /// <param name="ngrams1">The first n-gram collection.</param>
    /// <param name="ngrams2">The second n-gram collection.</param>
    /// <param name="requiredSimilarity">The required similarity.</param>
    /// <returns>
    /// <see langword="true"/> if the n-gram collections are similar enough.
    /// <see langword="false"/> otherwise.
    /// </returns>
    /// <seealso href="https://en.wikipedia.org/wiki/Dice-S%C3%B8rensen_coefficient"/>
    public static bool AtLeast(NGramsCollection ngrams1, NGramsCollection ngrams2, float requiredSimilarity)
    {
        if (ngrams1.Count == 0)
        {
            return ngrams2.Count == 0;
        }
        else if (ngrams2.Count == 0)
        {
            return false;
        }

        if (ngrams2.Count > ngrams1.Count)
        {
            (ngrams1, ngrams2) = (ngrams2, ngrams1);
        }

        var sizeDiffRatio = (float)ngrams2.Count / ngrams1.Count;

        if (sizeDiffRatio < requiredSimilarity)
        {
            return false;
        }

        var dcs = ngrams1.DiceSorensenCoefficient(ngrams2);

        return dcs >= requiredSimilarity;
    }
}
