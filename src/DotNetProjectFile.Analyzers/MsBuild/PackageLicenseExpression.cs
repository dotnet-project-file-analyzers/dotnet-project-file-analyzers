namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseExpression(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
