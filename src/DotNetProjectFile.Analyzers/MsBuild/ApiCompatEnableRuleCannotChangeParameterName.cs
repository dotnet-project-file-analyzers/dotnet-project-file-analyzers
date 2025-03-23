namespace DotNetProjectFile.MsBuild;

public sealed class ApiCompatEnableRuleCannotChangeParameterName(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
