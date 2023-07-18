namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference : Node
{
    public PackageReference(XElement element, Node parent, MsBuildProject project) : base(element, parent, project) { }

    public string? Include => Attribute();

    public string? Update => Attribute();

    public string? Version => Attribute();
}
