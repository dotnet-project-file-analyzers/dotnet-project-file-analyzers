namespace DotNetProjectFile.MsBuild;

public sealed class None(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
