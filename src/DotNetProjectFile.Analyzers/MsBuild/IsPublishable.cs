namespace DotNetProjectFile.MsBuild;

public sealed class IsPublishable(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
