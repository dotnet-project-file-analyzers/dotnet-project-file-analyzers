namespace DotNetProjectFile.Syntax;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public readonly struct SyntaxNodeCollection<TSyntax>(SyntaxNode parent)
    : IReadOnlyList<TSyntax> where TSyntax : SyntaxNode
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ImmutableArray<SyntaxNode> Children = parent.Children;

    /// <inheritdoc />
    public TSyntax this[int index] => Children.OfType<TSyntax>().Skip(index).First();

    /// <inheritdoc />
    public int Count => Children.OfType<TSyntax>().Count();

    [Pure]
    public ImmutableArray<SyntaxNode> WithLast(Func<TSyntax, TSyntax> update)
    {
        var trimmed = Children[..^1];
        var last = Children[^1] as TSyntax
            ?? throw new InvalidCastException($"Last child is of the type {Children[..^1].GetType()}, and not of the type {typeof(TSyntax)}.");
        var updated = update(last);
        return trimmed.Add(updated);
    }

    [Pure]
    public ImmutableArray<SyntaxNode> Add(TSyntax node) => Children.Add(node);

    /// <inheritdoc />
    [Pure]
    public IEnumerator<TSyntax> GetEnumerator() => Children.OfType<TSyntax>().GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
