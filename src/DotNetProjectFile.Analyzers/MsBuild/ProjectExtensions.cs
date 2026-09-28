namespace DotNetProjectFile.MsBuild;

public sealed class ProjectExtensions(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
