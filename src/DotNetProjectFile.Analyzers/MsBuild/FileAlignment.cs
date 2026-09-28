namespace DotNetProjectFile.MsBuild;

public sealed class FileAlignment(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
