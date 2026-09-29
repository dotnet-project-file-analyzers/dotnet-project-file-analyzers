namespace DotNetProjectFile.MsBuild;

public sealed class Install(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
