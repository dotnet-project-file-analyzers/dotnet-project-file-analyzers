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
}
