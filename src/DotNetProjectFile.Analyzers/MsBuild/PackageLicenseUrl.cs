namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseUrl(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
