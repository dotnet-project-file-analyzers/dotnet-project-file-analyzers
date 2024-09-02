namespace DotNetProjectFile.MsBuild;

public sealed class GlobalPackageReference(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Version => Attribute();

    public string? PrivateAssets => Attribute() ?? Child();
}
