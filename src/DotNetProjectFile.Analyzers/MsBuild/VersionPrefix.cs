namespace DotNetProjectFile.MsBuild;

public sealed class VersionPrefix(XElement element, Node parent, MsBuildProject project)
    : Node<SemVer>(element, parent, project);
