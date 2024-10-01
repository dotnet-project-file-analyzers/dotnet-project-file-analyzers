using DotNetProjectFile.Collections;

namespace System.Collections.Generic;

public static class ReadOnlyListExtensions
{
    [Pure]
    public static Slice<T> Slice<T>(this IReadOnlyList<T> list, SliceSpan span)
        => list is Slice<T> slice
            ? slice.Span(span)
            : new Slice<T>(span, list);

    [Pure]
    public static Slice<T> Slice<T>(this IReadOnlyList<T> list, int start, int size)
        => list.Slice(new SliceSpan(start, size));
}
