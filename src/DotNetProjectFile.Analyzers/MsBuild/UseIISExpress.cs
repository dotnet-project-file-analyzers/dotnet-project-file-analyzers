namespace DotNetProjectFile.MsBuild;

public sealed class UseIISExpress(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
