namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyTitle(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
