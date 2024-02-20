namespace DotNetProjectFile.MsBuild;

public sealed class PackageReadmeFile(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
