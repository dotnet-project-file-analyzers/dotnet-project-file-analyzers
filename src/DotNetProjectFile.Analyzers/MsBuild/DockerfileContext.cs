namespace DotNetProjectFile.MsBuild;

public sealed class DockerfileContext(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
