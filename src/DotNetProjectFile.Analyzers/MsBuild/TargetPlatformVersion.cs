namespace DotNetProjectFile.MsBuild;

public sealed class TargetPlatformVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
