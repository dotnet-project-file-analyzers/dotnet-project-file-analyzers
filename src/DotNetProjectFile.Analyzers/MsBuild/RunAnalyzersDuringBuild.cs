namespace DotNetProjectFile.MsBuild;

public sealed class RunAnalyzersDuringBuild(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project);
