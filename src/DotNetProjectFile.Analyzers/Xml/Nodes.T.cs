namespace DotNetProjectFile.Xml;

/// <summary>A read-only list of nodes.</summary>
/// <typeparam name="TNode">
/// The type of the XML node.
/// </typeparam>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct Nodes<TNode>(IEnumerable<XmlAnalysisNode> items) : IReadOnlyList<TNode>
    where TNode : class, XmlAnalysisNode
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEnumerable<XmlAnalysisNode> Items = items;

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
