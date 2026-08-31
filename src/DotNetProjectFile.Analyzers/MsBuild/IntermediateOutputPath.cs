namespace DotNetProjectFile.MsBuild;

public sealed class IntermediateOutputPath(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
