namespace DotNetProjectFile.MsBuild;

public sealed class GlobalAnalyzerConfigFiles(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project)
{ }
