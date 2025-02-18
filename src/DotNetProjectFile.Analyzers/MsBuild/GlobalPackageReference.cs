namespace DotNetProjectFile.MsBuild;

public sealed class GlobalPackageReference(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
}
