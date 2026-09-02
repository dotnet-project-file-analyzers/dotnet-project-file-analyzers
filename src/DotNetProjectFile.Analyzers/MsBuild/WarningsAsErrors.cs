namespace DotNetProjectFile.MsBuild;

public sealed class WarningsAsErrors(XElement element, Node parent, MsBuildProject project)
    : WarnBase(element, parent, project);
