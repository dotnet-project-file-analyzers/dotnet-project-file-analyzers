namespace DotNetProjectFile.MsBuild;

public sealed class BaseIntermediateOutputPath(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
