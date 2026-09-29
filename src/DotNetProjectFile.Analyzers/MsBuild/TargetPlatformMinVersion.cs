namespace DotNetProjectFile.MsBuild;

public sealed class TargetPlatformMinVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
