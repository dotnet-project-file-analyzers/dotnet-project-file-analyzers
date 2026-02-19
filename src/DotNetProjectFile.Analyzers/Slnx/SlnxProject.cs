namespace DotNetProjectFile.Slnx;

public sealed class SlnxProject(XElement element, Node parent, SolutionFile solution)
    : Node(element, parent, solution)
{
    public string? Path => Attribute();

    public string? Id => Attribute();

    /// <summary>Gets the local name `Project`.</summary>
    public override string LocalName => "Project";
}
