namespace DotNetProjectFile.MsBuild;

public sealed class PublishDir(XElement element, Node parent, MsBuildProject project)
    : Node<IODirectory?>(element, parent, project);
