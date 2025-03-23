namespace DotNetProjectFile.MsBuild;

public sealed class DevelopmentDependency(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
