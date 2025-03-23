namespace DotNetProjectFile.MsBuild;

public sealed class VersionSuffix(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
