namespace DotNetProjectFile.MsBuild;

public sealed class PackageId(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
