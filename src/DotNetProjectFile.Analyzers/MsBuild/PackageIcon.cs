namespace DotNetProjectFile.MsBuild;

public sealed class PackageIcon : StringValueNode
{
    public PackageIcon(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
