namespace DotNetProjectFile.Resx;

public sealed class Comment(XElement element, Resource resource) : Node(element, resource)
{
    public string? Text => Element.Value;
}
