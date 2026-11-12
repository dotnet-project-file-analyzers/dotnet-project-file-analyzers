namespace DotNetProjectFile.MsBuild;

public sealed class Page(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project)
{ }
