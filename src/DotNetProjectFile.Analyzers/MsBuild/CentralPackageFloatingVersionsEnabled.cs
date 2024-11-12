namespace DotNetProjectFile.MsBuild;

public sealed class CentralPackageFloatingVersionsEnabled(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project)
{ }
