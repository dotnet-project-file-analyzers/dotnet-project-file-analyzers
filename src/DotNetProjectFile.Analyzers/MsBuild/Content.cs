namespace DotNetProjectFile.MsBuild;

public sealed class Content(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project)
{ }
