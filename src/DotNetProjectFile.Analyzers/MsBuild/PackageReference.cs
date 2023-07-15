namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference : Node
{
    public PackageReference(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Include => Attribute();

    public string? Version => Attribute();
}
