namespace DotNetProjectFile.MsBuild;

public sealed class IsPublishable(XElement element, Node parent, Project project)
    : Node<bool?>(element, parent, project) { }
