namespace DotNetProjectFile.MsBuild;

public sealed class CscToolPath(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
