namespace DotNetProjectFile.MsBuild;

public sealed class PackageProjectUrl : Node<string>
{
    public PackageProjectUrl(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
