namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseUrl : Node<string>
{
    public PackageLicenseUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
