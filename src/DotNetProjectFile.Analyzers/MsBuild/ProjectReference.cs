namespace DotNetProjectFile.MsBuild;

public sealed class ProjectReference(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();
}
