namespace DotNetProjectFile.MsBuild;

public sealed class Compile(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
