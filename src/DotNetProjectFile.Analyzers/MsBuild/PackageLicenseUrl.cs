namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseUrl : StringValueNode
{
    public PackageLicenseUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
