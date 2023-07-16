namespace DotNetProjectFile.MsBuild;

public sealed class PackageReleaseNotes : StringValueNode
{
    public PackageReleaseNotes(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
