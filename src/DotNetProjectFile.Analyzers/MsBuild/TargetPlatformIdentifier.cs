namespace DotNetProjectFile.MsBuild;

public sealed class TargetPlatformIdentifier(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
