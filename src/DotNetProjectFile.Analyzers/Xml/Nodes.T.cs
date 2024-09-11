using System.Diagnostics.Contracts;

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

    /// <summary>Returns a subset of the nodes of the specified type.</summary>
    [Pure]
    public Nodes<TOut> OfType<TOut>() where TOut : class, TNode => new(Items.OfType<TOut>());

    [Pure]
    public Nodes<TOut> NestedTyped<TOut>() where TOut : class, TNode, XmlAnalysisNode
        => new(Items.SelectMany(n => n.DescendantsAndSelf()).OfType<TOut>());

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
