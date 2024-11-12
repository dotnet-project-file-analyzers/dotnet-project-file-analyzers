namespace DotNetProjectFile.MsBuild;

public sealed class IsPackable(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
