namespace DotNetProjectFile.MsBuild;

public sealed class CentralPackageTransitivePinningEnabled(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project)
{ }
