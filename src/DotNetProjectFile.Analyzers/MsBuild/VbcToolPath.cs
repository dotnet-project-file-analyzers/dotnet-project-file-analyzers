namespace DotNetProjectFile.MsBuild;

public sealed class VbcToolPath(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
