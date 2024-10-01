namespace DotNetProjectFile.Collections;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}, Offset = {Offset}")]
public readonly struct Slice<T>(int start, int size, IReadOnlyList<T> list) :
    IReadOnlyList<T>
{
    internal Slice(SliceSpan span, IReadOnlyList<T> list) : this(span.Start, span.Size, list) { }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyList<T> List = list;

    public int Count { get; } = size;

    public T this[int index] => List[index + Offset];

    private int Offset { get; } = start;

    [Pure]
    public Slice<T> Span(SliceSpan span)
        => new(Offset + span.Start, span.Size, List);

    /// <inheritdoc />
    [Pure]

    public IEnumerator<T> GetEnumerator() => List.Skip(Offset).Take(Count).GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
