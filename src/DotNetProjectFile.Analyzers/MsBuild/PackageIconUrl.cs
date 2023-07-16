namespace DotNetProjectFile.MsBuild;

public sealed class PackageIconUrl : Node<string>
{
    public PackageIconUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
