namespace DotNetProjectFile.MsBuild;

public sealed class EmbeddedResource(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project) { }
