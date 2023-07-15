namespace DotNetProjectFile.MsBuild;

public sealed class Version : Node
{
    public Version(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
