namespace DotNetProjectFile.MsBuild;

public sealed class PackageIconUrl(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
