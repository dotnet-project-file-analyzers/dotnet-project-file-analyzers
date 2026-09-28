namespace DotNetProjectFile.MsBuild;

public sealed class StartAction(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
