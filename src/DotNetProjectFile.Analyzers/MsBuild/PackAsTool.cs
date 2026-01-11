namespace DotNetProjectFile.MsBuild;

public sealed class PackAsTool(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
