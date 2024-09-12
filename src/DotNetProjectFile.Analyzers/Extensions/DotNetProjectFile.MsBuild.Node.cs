using DotNetProjectFile.Navigation;

namespace DotNetProjectFile.MsBuild;

public static class NodeExtensions
{
    public static IEnumerable<Node> Walk(this Node node)
        => Walk(node, new ProjectTrace(node.Project.Path));

    private static IEnumerable<Node> Walk(Node node, ProjectTrace trace)
    {
        yield return node;

        if (node is Import import
            && import.Value is { } imported
            && !trace.Contains(imported.Path))
        {
            foreach (var child in Walk(imported, trace.Append(imported.Path)))
            {
                yield return child;
            }
        }

        foreach (var child in node.Children.SelectMany(c => Walk(c, trace)))
        {
            yield return child;
        }
    }
}
