namespace DotNetProjectFile.MsBuild;

public sealed class PackageIcon(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
