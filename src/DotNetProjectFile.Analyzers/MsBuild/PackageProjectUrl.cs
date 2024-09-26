namespace DotNetProjectFile.MsBuild;

public sealed class PackageProjectUrl(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
