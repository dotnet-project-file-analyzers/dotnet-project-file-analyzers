namespace DotNetProjectFile.MsBuild;

public sealed class Authors(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
