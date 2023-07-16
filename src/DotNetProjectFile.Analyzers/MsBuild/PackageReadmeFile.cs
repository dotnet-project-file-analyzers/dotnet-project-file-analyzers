namespace DotNetProjectFile.MsBuild;

public sealed class PackageReadmeFile : Node<string>
{
    public PackageReadmeFile(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
