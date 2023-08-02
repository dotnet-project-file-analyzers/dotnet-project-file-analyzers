namespace DotNetProjectFile.MsBuild;

internal static class Nodes
{
    public static IEnumerable<Node> Concat(IEnumerable<Node> first, IEnumerable<Node> second) => first.Concat(second);
}
