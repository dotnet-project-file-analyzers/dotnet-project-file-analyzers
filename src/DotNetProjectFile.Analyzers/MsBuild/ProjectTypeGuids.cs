namespace DotNetProjectFile.MsBuild;

public sealed class ProjectTypeGuids(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
