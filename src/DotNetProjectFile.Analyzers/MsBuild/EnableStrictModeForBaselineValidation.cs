namespace DotNetProjectFile.MsBuild;

public sealed class EnableStrictModeForBaselineValidation(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
