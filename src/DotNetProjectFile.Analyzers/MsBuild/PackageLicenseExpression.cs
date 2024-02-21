namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseExpression(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) { }
