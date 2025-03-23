namespace DotNetProjectFile.MsBuild;

public sealed class ApiCompatEnableRuleAttributesMustMatch(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
