namespace DotNetProjectFile.MsBuild;

public sealed class Compile(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project) { }
