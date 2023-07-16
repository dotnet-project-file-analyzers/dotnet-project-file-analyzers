namespace DotNetProjectFile.MsBuild;

public sealed class PackageIcon : Node<string>
{
    public PackageIcon(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
