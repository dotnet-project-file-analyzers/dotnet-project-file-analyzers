namespace DotNetProjectFile.MsBuild;

public sealed class PackageTags(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
