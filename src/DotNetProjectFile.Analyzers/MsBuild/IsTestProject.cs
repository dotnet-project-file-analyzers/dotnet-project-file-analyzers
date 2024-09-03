namespace DotNetProjectFile.MsBuild;

public sealed class IsTestProject(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
