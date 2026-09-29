namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyFileVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
