namespace DotNetProjectFile.MsBuild;

public sealed class PackageReleaseNotes : Node<string>
{
    public PackageReleaseNotes(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
