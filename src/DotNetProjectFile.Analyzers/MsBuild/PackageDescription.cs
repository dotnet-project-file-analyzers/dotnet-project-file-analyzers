namespace DotNetProjectFile.MsBuild;

public sealed class PackageDescription(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
