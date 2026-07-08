namespace Grammr;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public sealed class GrammrChildren(int capacity = 0) : IReadOnlyList<GrammrNode>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<GrammrNode> Collection = new(capacity);

    /// <inheritdoc />
    public GrammrNode this[int index] => Collection[index];

    /// <inheritdoc />
    public int Count => Collection.Count;

    /// <summary>Adds a node to the list.</summary>
    internal void Add(GrammrNode node) => Collection.Add(node);

    /// <summary>Adds a node to the list.</summary>
    internal void AddRange(IEnumerable<GrammrNode> nodes) => Collection.AddRange(nodes);

    /// <inheritdoc />
    [Pure]
    public IEnumerator<GrammrNode> GetEnumerator() => Collection.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
