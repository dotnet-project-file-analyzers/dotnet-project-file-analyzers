namespace DotNetProjectFile.MsBuild;

public sealed class BaseOutputPath(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
