namespace DotNetProjectFile.Resx;

public sealed class Value(XElement element, Resource resource) : Node(element, resource)
{
    public string? Text => Element.Value;
}
