namespace DotNetProjectFile.MsBuild;

public sealed class PackageOutputPath(XElement element, Node parent, MsBuildProject project)
    : Node<IODirectory?>(element, parent, project);
