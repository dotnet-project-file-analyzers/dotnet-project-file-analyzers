namespace DotNetProjectFile.MsBuild;

public sealed class PackageProjectUrl(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
