namespace DotNetProjectFile.MsBuild;

public sealed class None(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project)
{ }
