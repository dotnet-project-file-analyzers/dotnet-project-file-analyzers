using DotNetProjectFile.Navigation;

namespace DotNetProjectFile.MsBuild;

public sealed partial class Project
{
    /// <summary>Walks through all nodes.</summary>
    public IEnumerable<Node> Walk()
        => SelfAndDirectoryProps()
        .Reverse()
        .SelectMany(p => Walk(p, new(p.Path)));

    /// <summary>Walks through all nodes backwards.</summary>
    public IEnumerable<Node> WalkBackward()
        => SelfAndDirectoryProps()
        .SelectMany(p => WalkBackward(p, new(p.Path)));

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

    private static IEnumerable<Node> WalkBackward(Node node, ProjectTrace trace)
    {
        for (var i = node.Children.Count - 1; i >= 0; i--)
        {
            var child = node.Children[i];

            foreach (var ancestor in WalkBackward(child, trace))
            {
                yield return ancestor;
            }
        }

        if (node is Import import
            && import.Value is { } imported
            && !trace.Contains(imported.Path))
        {
            foreach (var child in WalkBackward(imported, trace.Append(imported.Path)))
            {
                yield return child;
            }
        }

        yield return node;
    }
}
