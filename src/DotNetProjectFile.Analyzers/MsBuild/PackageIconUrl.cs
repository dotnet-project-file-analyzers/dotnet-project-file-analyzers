namespace DotNetProjectFile.MsBuild;

public sealed class PackageIconUrl(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
