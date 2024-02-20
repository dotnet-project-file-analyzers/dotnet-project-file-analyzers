namespace DotNetProjectFile.MsBuild;

public sealed class Choose(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project) { }
