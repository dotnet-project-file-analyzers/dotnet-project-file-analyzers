namespace DotNetProjectFile.MsBuild;

public sealed class EmbeddedResource(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
