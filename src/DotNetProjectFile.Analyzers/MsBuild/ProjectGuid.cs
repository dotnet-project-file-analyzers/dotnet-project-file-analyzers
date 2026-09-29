namespace DotNetProjectFile.MsBuild;

public sealed class ProjectGuid(XElement element, Node parent, MsBuildProject project)
    : Node<SemVer>(element, parent, project);
