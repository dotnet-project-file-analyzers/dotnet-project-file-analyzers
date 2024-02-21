namespace DotNetProjectFile.MsBuild;

public sealed class IsPackable(XElement element, Node parent, Project project)
    : Node<bool?>(element, parent, project) { }
