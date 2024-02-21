namespace DotNetProjectFile.MsBuild;

public sealed class Authors(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
