namespace DotNetProjectFile.MsBuild;

public sealed class PackageReadmeFile : StringValueNode
{
    public PackageReadmeFile(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
