namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseFile : StringValueNode
{
    public PackageLicenseFile(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
