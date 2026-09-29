namespace DotNetProjectFile.MsBuild;

public sealed class UseGlobalApplicationHostFile(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
