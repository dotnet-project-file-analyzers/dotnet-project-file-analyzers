namespace Grammr;

/// <summary>A read-only list of nodes.</summary>
/// <typeparam name="TNode">
/// The type of the node.
/// </typeparam>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct GrammrNodes<TNode>(IEnumerable<GrammrNode> items) : IReadOnlyList<TNode>
    where TNode : GrammrNode
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEnumerable<GrammrNode> Items = items;

    /// <inheritdoc />
    public TNode this[int index] => this.Skip(index).First();

    /// <inheritdoc />
    public int Count => Items.OfType<TNode>().Count();

    /// <inheritdoc />
    [Pure]
    public IEnumerator<TNode> GetEnumerator() => Items.OfType<TNode>().GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage/* Justification = "Debug experience only." */]
    private string DebuggerDisplay => $"{typeof(TNode).Name}, Count = {Count}";
}
