namespace System.Linq;

internal static class EnumerableExtensions
{
    public static bool None<T>(this IEnumerable<T> enumerable)
        => !enumerable.Any();

    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        => !enumerable.Any(predicate);

    /// <summary>Takes all elements until (and including the first matching element).</summary>
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        var match = false;

        foreach (T element in enumerable)
        {
            if (match)
            {
                break;
            }
            match = predicate(element);

            yield return element;
        }
    }

    /// <summary>Returns the first matching element, or null if no match was found.</summary>
    [Pure]
    public static T? FirstOrNone<T>(this IEnumerable<T> enumerable, Predicate<T> predicate) where T : struct
    {
        foreach (var element in enumerable)
        {
            if (predicate(element))
            {
                return element;
            }
        }
        return null;
    }
}
