namespace System.Linq;

internal static class EnumerableExtensions
{
    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        => !enumerable.Any(predicate);
}
