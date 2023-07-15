namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseUrl : Node
{
    public PackageLicenseUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
