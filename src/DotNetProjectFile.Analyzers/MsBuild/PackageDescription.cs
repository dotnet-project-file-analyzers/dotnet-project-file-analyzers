namespace DotNetProjectFile.MsBuild;

public sealed class PackageDescription(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
