namespace DotNetProjectFile.MsBuild;

public sealed class ApiCompatGenerateSuppressionFile(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
