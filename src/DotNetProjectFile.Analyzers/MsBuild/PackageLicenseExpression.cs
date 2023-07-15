namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseExpression : Node
{
    public PackageLicenseExpression(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
