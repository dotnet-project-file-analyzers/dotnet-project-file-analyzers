namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyInformationalVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
