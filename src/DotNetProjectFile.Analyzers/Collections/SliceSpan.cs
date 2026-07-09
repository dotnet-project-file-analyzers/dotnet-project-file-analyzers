namespace DotNetProjectFile.Collections;

/// <summary>Describes the span/range of a slice.</summary>
/// <param name="Start">
/// Gets the start index of the span.
/// </param>
/// <param name="Size">
/// Gets the size of the span.
/// </param>
[DebuggerDisplay("[{Start}..{End}], Size = {Size}")]
public readonly record struct SliceSpan(int Start, int Size)
{
    /// <summary>Gets the end of the span.</summary>
    public int End => Start + Size;

    /// <summary>Calculates the delta of two collections to get the slice span.</summary>
    [Pure]
    public static SliceSpan Delta<T>(IReadOnlyList<T> curr, IReadOnlyList<T> prev)
        => new(prev.Count, curr.Count - prev.Count);
}
