namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyDescription(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
