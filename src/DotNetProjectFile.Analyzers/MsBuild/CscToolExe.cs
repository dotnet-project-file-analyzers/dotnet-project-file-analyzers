namespace DotNetProjectFile.MsBuild;

public sealed class CscToolExe(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
