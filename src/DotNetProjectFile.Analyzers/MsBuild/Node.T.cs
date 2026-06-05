namespace DotNetProjectFile.MsBuild;

/// <summary>Represent a node in an MSBuild file.</summary>
public abstract class Node<T>(XElement element, Node? parent, MsBuildProject? project)
    : Node(element, parent, project)
{
    /// <summary>The value of the node.</summary>
    public sealed override object? Val => Value;

    /// <summary>The typed value of the node.</summary>
    public virtual T? Value => Convert<T?>(Text);

    /// <summary>The text content of the node.</summary>
    public string Text => Element.Value;
}
