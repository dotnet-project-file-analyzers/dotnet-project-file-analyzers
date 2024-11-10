namespace DotNetProjectFile.MsBuild;

public sealed class ProductName(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
