namespace DotNetProjectFile.MsBuild;

public sealed class TargetFrameworkProfile(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
