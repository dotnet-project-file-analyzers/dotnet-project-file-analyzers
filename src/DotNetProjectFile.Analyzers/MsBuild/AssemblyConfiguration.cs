namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyConfiguration(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
