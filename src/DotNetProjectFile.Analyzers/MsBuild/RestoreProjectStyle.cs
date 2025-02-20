namespace DotNetProjectFile.MsBuild;

public sealed class RestoreProjectStyle(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
