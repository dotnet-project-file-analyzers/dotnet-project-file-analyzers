namespace DotNetProjectFile.MsBuild;

public sealed class VersionPrefix(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
