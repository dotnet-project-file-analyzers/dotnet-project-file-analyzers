namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseExpression : Node<string>
{
    public PackageLicenseExpression(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
