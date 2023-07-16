namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseFile : Node<string>
{
    public PackageLicenseFile(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
