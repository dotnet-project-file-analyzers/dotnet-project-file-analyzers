namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyCopyright(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
