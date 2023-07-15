namespace DotNetProjectFile.MsBuild;

public sealed class PackageIconUrl : Node
{
    public PackageIconUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
