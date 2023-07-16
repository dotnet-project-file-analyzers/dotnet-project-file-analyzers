namespace DotNetProjectFile.MsBuild;

public sealed class PackageTags : StringValueNode
{
    public PackageTags(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
