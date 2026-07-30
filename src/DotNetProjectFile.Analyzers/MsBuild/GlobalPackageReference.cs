namespace DotNetProjectFile.MsBuild;

public sealed class GlobalPackageReference(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
    public override (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled)
        => Version is { Length: > 0 } v ? (this, v) : null;
}
