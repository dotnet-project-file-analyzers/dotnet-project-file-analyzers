namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Update => Attribute();

    public string? Version => Attribute();

    public string IncludeOrUpdate => Include ?? Update ?? string.Empty;

    public string? PrivateAssets => Attribute() ?? Child();
}
