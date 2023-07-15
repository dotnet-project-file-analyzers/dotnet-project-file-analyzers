namespace DotNetProjectFile.MsBuild;

public sealed class Description : Node
{
    public Description(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
