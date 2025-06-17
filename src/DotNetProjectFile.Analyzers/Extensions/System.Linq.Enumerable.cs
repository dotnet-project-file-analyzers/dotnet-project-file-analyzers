namespace System.Linq;

internal static class EnumerableExtensions
{
    [Pure]
    public static bool None<T>(this IEnumerable<T> enumerable)
        => !enumerable.Any();

    [Pure]
    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        => !enumerable.Any(predicate);

    /// <summary>Takes all elements until (and including the first matching element).</summary>
    [Pure]
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

    /// <summary>Returns the first matching element, or null if no match was found.</summary>
    [Pure]
    public static T? FirstOrNone<T>(this IEnumerable<T> enumerable) where T : struct
    {
        using var enumerator = enumerable.GetEnumerator();
        return enumerator.MoveNext()
            ? enumerator.Current
            : null;
    }

    [Pure]
    public static T MaxBy<T, TComparable>(this IEnumerable<T> enumerable, Func<T, TComparable> getValue)
        where TComparable : IComparable<TComparable>
    {
        var result = enumerable.First();
        var resultValue = getValue(result);

        foreach (var item in enumerable.Skip(1))
        {
            var value = getValue(item);

            if (value.CompareTo(value) > 0)
            {
                result = item;
                resultValue = value;
            }
        }

        return result;
    }

    [Pure]
    public static T MinBy<T, TComparable>(this IEnumerable<T> enumerable, Func<T, TComparable> getValue)
    where TComparable : IComparable<TComparable>
    {
        var result = enumerable.First();
        var resultValue = getValue(result);

        foreach (var item in enumerable.Skip(1))
        {
            var value = getValue(item);

            if (value.CompareTo(value) < 0)
            {
                result = item;
                resultValue = value;
            }
        }

        return result;
    }
}
