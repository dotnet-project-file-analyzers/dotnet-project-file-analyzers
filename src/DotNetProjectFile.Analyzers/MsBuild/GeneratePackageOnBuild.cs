namespace DotNetProjectFile.MsBuild;

public sealed class GeneratePackageOnBuild(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
