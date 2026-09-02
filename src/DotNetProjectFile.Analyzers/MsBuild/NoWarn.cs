namespace DotNetProjectFile.MsBuild;

public sealed class NoWarn(XElement element, Node parent, MsBuildProject project)
    : WarnBase(element, parent, project);
