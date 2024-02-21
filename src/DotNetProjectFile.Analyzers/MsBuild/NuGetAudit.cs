namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAudit(XElement element, Node parent, Project project)
    : Node<bool?>(element, parent, project) { }
