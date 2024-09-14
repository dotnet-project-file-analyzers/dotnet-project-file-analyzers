namespace DotNetProjectFile.Xml;

internal static class Nodes
{
    /// <inheritdoc cref="Enumerable.Concat{TSource}(IEnumerable{TSource}, IEnumerable{TSource})" />
    [Pure]
    public static IEnumerable<Node> Concat(IEnumerable<Node> first, IEnumerable<Node> second) => first.Concat(second);
}
