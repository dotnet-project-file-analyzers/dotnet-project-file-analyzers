namespace DotNetProjectFile.MsBuild;

public sealed class IncludeSymbols(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
