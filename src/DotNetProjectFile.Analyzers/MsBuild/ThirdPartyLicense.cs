namespace DotNetProjectFile.MsBuild;

public sealed class ThirdPartyLicense(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Version => Attribute();

    public string? Hash => Attribute();
}
