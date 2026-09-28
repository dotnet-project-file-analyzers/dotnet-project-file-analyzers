namespace DotNetProjectFile.MsBuild;

public sealed class SchemaVersion(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project);
