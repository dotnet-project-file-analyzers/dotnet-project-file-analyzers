namespace DotNetProjectFile.MsBuild;

public abstract class Node<T> : Node
{
    protected Node(XElement element, Node? parent, Project? project)
        : base(element, parent, project) { }

    public sealed override object? Val => Value;

    public virtual T? Value => Convert<T?>(Element.Value);
}
