namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseExpression : StringValueNode
{
    public PackageLicenseExpression(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
