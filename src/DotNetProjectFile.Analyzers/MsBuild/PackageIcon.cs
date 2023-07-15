namespace DotNetProjectFile.MsBuild;

public sealed class PackageIcon : Node
{
    public PackageIcon(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
