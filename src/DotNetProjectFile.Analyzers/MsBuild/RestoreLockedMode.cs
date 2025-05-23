namespace DotNetProjectFile.MsBuild;

public sealed class RestoreLockedMode(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
