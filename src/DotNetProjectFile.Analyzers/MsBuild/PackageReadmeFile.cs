namespace DotNetProjectFile.MsBuild;

public sealed class PackageReadmeFile(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
