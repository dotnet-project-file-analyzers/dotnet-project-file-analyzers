namespace DotNetProjectFile.MsBuild;

public sealed class OutputPath(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
