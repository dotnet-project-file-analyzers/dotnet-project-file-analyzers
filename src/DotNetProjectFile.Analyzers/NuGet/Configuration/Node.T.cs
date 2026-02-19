namespace DotNetProjectFile.NuGet.Configuration;

public abstract class Node<T>(XElement element, Node? parent, NuGetConfigFile? configFile)
    : Node(element, parent, configFile)
{
    public sealed override object? Val => Value;

    public virtual T? Value => Convert<T?>(Element.Value);
}
