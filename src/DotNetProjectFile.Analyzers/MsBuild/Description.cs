namespace DotNetProjectFile.MsBuild;

public sealed class Description(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
