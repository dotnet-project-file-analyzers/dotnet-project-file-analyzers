namespace DotNetProjectFile.MsBuild;

public sealed class ManagePackageVersionsCentrally(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project)
{ }
