namespace DotNetProjectFile.Slnx;

public sealed class Project(XElement element, Node parent, SolutionFile solution)
    : Node(element, parent, solution)
{
    public string? Path => Attribute();

    public string? Id => Attribute();
}
