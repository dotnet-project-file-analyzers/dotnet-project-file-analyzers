namespace Grammr.Syntax;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public readonly struct Nodes<TSyntax>(Node parent)
    : IReadOnlyList<TSyntax> where TSyntax : Node
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyList<Node> Children = parent.Children;

    /// <inheritdoc />
    public TSyntax this[int index] => Children.OfType<TSyntax>().Skip(index).First();

    /// <inheritdoc />
    public int Count => Children.OfType<TSyntax>().Count();

    /// <inheritdoc />
    [Pure]
    public IEnumerator<TSyntax> GetEnumerator() => Children.OfType<TSyntax>().GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

