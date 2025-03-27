namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseUrl(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
