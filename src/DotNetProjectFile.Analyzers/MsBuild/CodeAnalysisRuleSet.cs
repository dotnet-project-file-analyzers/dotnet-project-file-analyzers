namespace DotNetProjectFile.MsBuild;

public sealed class CodeAnalysisRuleSet(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile>(element, parent, project)
{ }
