namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
