namespace DotNetProjectFile.MsBuild;

public sealed class Version(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
