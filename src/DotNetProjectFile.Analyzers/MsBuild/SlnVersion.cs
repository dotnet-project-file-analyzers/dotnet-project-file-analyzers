namespace DotNetProjectFile.MsBuild;

public sealed class SlnVersion(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
