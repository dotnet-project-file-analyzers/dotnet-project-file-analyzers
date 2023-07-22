namespace DotNetProjectFile.Analyzers.Helpers;

public static class CheckAlphabeticalOrderExtensions
{
    public static void CheckAlphabeticalOrder<T>(this IEnumerable<T> enumerable, Func<T, string?> getName, Action<T, T> onError)
    {
        var expectedOrder = enumerable.OrderBy(element => getName(element) ?? string.Empty, FileSystem.PathCompare);
        var pairing = enumerable
            .Zip(expectedOrder, (found, expected) => (expected, found));

        if (pairing.HasDifference(out var expected, out var found))
        {
            onError(expected, found);
        }
    }

    private static bool HasDifference<T>(this IEnumerable<(T Expected, T Found)> values, out T expected, out T found)
    {
        foreach ((var eExpected, var eFound) in values)
        {
            if (!Equals(eExpected, eFound))
            {
                expected = eExpected;
                found = eFound;
                return true;
            }
        }

        expected = default!;
        found = default!;
        return false;
    }
}
