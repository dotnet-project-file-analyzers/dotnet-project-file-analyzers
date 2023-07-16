namespace DotNetProjectFile.MsBuild;

public sealed class PackageIconUrl : StringValueNode
{
    public PackageIconUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
