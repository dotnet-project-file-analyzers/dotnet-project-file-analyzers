namespace DotNetProjectFile.MsBuild;

public sealed class UseVSToolPath(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
