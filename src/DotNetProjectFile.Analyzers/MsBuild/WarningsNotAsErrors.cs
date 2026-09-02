namespace DotNetProjectFile.MsBuild;

public sealed class WarningsNotAsErrors(XElement element, Node parent, MsBuildProject project)
    : WarnBase(element, parent, project);
