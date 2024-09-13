namespace System;

internal static class ArrayExtensions
{
    /// <inheritdoc cref="Array.Exists{T}(T[], Predicate{T})"/>
    [Pure]
    public static bool Exists<T>(this T[] array, Predicate<T> match)
        => Array.Exists(array, match);
}
