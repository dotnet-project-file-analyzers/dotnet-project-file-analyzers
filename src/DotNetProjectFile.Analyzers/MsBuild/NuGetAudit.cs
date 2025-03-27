namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAudit(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
