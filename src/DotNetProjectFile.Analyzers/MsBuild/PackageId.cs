namespace DotNetProjectFile.MsBuild;

public sealed class PackageId(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
