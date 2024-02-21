namespace DotNetProjectFile.MsBuild;

public sealed class Description(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
