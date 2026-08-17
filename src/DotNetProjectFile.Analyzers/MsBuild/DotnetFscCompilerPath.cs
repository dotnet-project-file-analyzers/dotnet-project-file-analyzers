namespace DotNetProjectFile.MsBuild;

public sealed class DotnetFscCompilerPath(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project);
