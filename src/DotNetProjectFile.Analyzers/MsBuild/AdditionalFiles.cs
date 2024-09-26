namespace DotNetProjectFile.MsBuild;

public sealed class AdditionalFiles(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project) { }
