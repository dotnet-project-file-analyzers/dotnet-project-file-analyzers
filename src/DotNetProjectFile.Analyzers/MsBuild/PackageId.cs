namespace DotNetProjectFile.MsBuild;

public sealed class PackageId : Node<string>
{
    public PackageId(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
