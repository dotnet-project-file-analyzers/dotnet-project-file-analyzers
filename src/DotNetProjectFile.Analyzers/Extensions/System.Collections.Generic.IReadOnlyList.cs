using DotNetProjectFile.Collections;

namespace System.Collections.Generic;

internal static class ReadOnlyListExtensions
{
    [Pure]
    public static Slice<T> AsSlice<T>(this IReadOnlyList<T> list) => new(0, list.Count, list);
}
