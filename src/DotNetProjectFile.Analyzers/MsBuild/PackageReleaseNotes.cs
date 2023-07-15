namespace DotNetProjectFile.MsBuild;

public sealed class PackageReleaseNotes : Node
{
    public PackageReleaseNotes(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
