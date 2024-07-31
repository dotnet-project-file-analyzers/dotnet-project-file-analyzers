namespace DotNetProjectFile.MsBuild;

public sealed class EnableNETAnalyzers(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project);
