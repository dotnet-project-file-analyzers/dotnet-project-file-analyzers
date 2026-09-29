namespace DotNetProjectFile.MsBuild;

public sealed class BootstrapperPackage(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
