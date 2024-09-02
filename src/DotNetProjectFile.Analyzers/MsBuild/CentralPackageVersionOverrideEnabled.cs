namespace DotNetProjectFile.MsBuild;

public sealed class CentralPackageVersionOverrideEnabled(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project)
{ }
