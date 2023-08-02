namespace DotNetProjectFile.MsBuild;

public sealed class PackageDescription : Node<string>
{
    public PackageDescription(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
