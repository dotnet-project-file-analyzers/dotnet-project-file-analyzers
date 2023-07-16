namespace DotNetProjectFile.MsBuild;

public sealed class PackageProjectUrl : StringValueNode
{
    public PackageProjectUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
