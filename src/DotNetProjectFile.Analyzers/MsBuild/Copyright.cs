namespace DotNetProjectFile.MsBuild;

public sealed class Copyright(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
