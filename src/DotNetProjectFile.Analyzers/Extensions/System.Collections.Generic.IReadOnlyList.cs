using DotNetProjectFile.Collections;

namespace System.Collections.Generic;

internal static class ReadOnlyListExtensions
{
    extension<T>(IReadOnlyList<T> list)
    {
        [Pure]
        public Slice<T> AsSlice() => new(0, list.Count, list);
    }
}
