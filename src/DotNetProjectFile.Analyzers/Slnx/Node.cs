namespace DotNetProjectFile.Slnx;

public abstract class Node : XmlAnalysisNode
{
    /// <summary>Initializes a new instance of the <see cref="Node"/> class.</summary>
    protected Node(XElement element, Node? parent, Solution? solution)
    {
        Element = element;
        Parent = parent;
        Solution = solution ?? (this as Solution) ?? throw new ArgumentNullException(nameof(solution));
        Locations = XmlPositions.New(element).Locations(Solution);
        Depth = element.Depth();
    }

    public Solution Solution { get; }

    public XElement Element { get; }

    public Node? Parent { get; }

    public int Depth { get; }

    public XmlLocations Locations { get; }

    public virtual object? Val => Element.Value;

    public virtual string LocalName => GetType().Name;

    public IEnumerable<XmlAnalysisNode> Children()
    {
        throw new NotImplementedException();
    }
}
