namespace DotNetProjectFile.MsBuild;

public sealed class AssemblyCompany(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
