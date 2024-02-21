namespace DotNetProjectFile.MsBuild;

public sealed class GeneratePackageOnBuild(XElement element, Node parent, Project project)
    : Node<bool?>(element, parent, project) { }
