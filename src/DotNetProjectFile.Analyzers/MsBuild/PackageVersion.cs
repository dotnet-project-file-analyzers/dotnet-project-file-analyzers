namespace DotNetProjectFile.MsBuild;

public sealed class PackageVersion(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
    public override (Node Node, string Version)? ResolveVersionVerbose(bool cpmEnabled)
        => (this, Version!);
}
