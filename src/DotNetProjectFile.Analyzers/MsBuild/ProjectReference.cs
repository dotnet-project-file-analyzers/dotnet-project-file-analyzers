namespace DotNetProjectFile.MsBuild;

public sealed class ProjectReference : Node
{
    public ProjectReference(XElement element, Node parent, MsBuildProject project) : base(element, parent, project) { }

    public string? Include => Attribute();
}
