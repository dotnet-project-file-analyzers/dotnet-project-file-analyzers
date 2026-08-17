namespace DotNetProjectFile.MsBuild;

public sealed class VbcToolExe(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
