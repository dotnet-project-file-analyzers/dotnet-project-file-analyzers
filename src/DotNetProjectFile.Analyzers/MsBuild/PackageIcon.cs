namespace DotNetProjectFile.MsBuild;

public sealed class PackageIcon(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
