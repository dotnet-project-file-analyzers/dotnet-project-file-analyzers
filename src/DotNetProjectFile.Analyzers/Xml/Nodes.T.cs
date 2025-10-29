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

    /// <summary>Gets the children of all nodes.</summary>
    [Pure]
    public Nodes<TChild> Children<TChild>(Predicate<TChild> predicate) where TChild : class, XmlAnalysisNode
        => new(Children<TChild>().Items.Where(child => predicate((TChild)child)));

    /// <summary>Gets all the children of all nodes.</summary>
    [Pure]
    public Nodes<TChild> Children<TChild>() where TChild : class, XmlAnalysisNode
        => new(Items
            .OfType<TNode>()
            .SelectMany(item => item.Children().OfType<TChild>()));

#pragma warning disable S1133 // Deprecated code should be removed
    // Prevents the use of LINQ when there are better alternatives.
    [Obsolete("Use Children<T>() instead.", true)]
    [DoesNotReturn]
    public IEnumerable<TResult> SelectMany<TResult>(Func<TNode, IEnumerable<TResult>> selector)
        => throw new NotSupportedException();

    [Obsolete("Use Children<T>(predicate) instead.", true)]
    [DoesNotReturn]
    public IEnumerable<TResult> Where<TResult>(Predicate<bool> predicate)
        => throw new NotSupportedException();
#pragma warning restore S1133 // Deprecated code should be removed

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
