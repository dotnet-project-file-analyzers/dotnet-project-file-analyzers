namespace DotNetProjectFile.Text;

/// <summary>
/// Measures the difference between two strings.
/// Uses the Levenshtein string difference algorithm.
///
/// See: https://en.wikipedia.org/wiki/Levenshtein_distance.
/// </summary>
/// <remarks>
/// Copyright: Dan Hartley https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE.
/// </remarks>
[DebuggerDisplay("{Value}")]
public readonly struct Levenshtein
{
    private readonly string Value;
    private readonly int[] Costs;

    /// <summary>Initializes a new instance of the <see cref="Levenshtein"/> struct.</summary>
    public Levenshtein(string value)
    {
        Value = value;
        Costs = new int[value.Length];
    }

    /// <summary>Compares another value to this value.</summary>
    [Pure]
    public int DistanceFrom(string other)
    {
        // copying to local variables allows JIT to remove bounds checks, as it understands they can not change
        var costs = Costs;
        var value = Value;

        // this will never be true, however it allows the JIT to remove a bounds check.
        if (costs.Length != value.Length) { return int.MaxValue; }

        // Add indexing for insertion to first row
        for (var i = 0; i < costs.Length;)
        {
            costs[i] = ++i;
        }

        for (var i = 0; i < other.Length; i++)
        {
            // cost of the first index
            var cost = i;
            var prev = i;

            // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
            var ch = other[i];

            for (var j = 0; j < value.Length; j++)
            {
                var curr = cost;

                // assigning this here reduces the array reads we do, improvement of the old version
                cost = costs[j];

                if (ch != value[j])
                {
                    if (prev < curr)
                    {
                        curr = prev;
                    }

                    if (cost < curr)
                    {
                        curr = cost;
                    }

                    ++curr;
                }
                // Swapping the variables here results in a performance improvement for modern Intel CPU’s.
                costs[j] = curr;
                prev = curr;
            }
        }
        return costs[^1];
    }
}
