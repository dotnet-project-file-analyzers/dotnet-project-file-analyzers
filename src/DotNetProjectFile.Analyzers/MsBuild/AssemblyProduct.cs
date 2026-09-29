namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyProduct(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
