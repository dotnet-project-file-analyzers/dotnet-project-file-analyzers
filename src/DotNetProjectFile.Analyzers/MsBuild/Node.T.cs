namespace DotNetProjectFile.MsBuild;

public abstract class Node<T>(XElement element, Node? parent, MsBuildProject? project)
    : Node(element, parent, project)
{
    public sealed override object? Val => Value;

    public virtual T? Value => Convert<T?>(Element.Value);
}
