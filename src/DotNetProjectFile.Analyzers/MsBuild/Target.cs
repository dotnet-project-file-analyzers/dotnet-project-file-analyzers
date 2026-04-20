namespace DotNetProjectFile.MsBuild;

public sealed class Target(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Name => Attribute();

    public string? DependsOn => Attribute();
}
