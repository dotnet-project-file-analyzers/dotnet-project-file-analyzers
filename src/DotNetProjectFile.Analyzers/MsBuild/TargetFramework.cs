namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
