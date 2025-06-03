namespace DotNetProjectFile.Slnx;

public abstract class Node<T>(XElement element, Node? parent, Solution? solution)
    : Node(element, parent, solution)
{
    public sealed override object? Val => Value;

    public virtual T? Value => Convert<T?>(Element.Value);
}
