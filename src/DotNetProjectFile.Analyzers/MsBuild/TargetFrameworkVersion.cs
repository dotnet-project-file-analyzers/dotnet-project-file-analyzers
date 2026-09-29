namespace DotNetProjectFile.MsBuild;

public sealed class TargetFrameworkVersion(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
