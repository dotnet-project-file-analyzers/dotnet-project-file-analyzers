namespace DotNetProjectFile.MsBuild;

public sealed class HintPath(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
