namespace DotNetProjectFile.MsBuild;

public sealed class Content(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
