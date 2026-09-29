namespace DotNetProjectFile.MsBuild;

public sealed class InstallFrom(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
