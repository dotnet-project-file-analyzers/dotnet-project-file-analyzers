namespace DotNetProjectFile.MsBuild;

public sealed class When(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project) { }
