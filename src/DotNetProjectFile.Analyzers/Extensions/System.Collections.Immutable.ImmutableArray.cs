namespace System.Collections.Immutable;

internal static class ImmutableArrayExtensions
{
    [Pure]
    public static ImmutableArray<T> WithLast<T>(this ImmutableArray<T> array, Func<T, T> update)
    {
        var trimmed = array[..^1];
        var updated = update(array[^1]);
        return trimmed.Add(updated);
    }
}
