using DotNetProjectFile.Xml;

namespace DotNetProjectFile.Resx;

public partial class Node : XmlAnalysisNode
{
    protected Node(XElement element, Resource? resource)
    {
        Element = element;
        Resource = resource ?? (this as Resource) ?? throw new ArgumentNullException(nameof(resource));
        Positions = XmlPositions.New(element);
        Depth = element.Ancestors().Count();
    }

    public XElement Element { get; }

    public Resource Resource { get; }

    public int Depth { get; }

    public XmlPositions Positions { get; }

    public string LocalName => Element.Name.LocalName;

    /// <summary>Gets the a <see cref="Nodes{T}"/> of children.</summary>
    public Nodes<T> Children<T>() where T : Node => new(this);

    /// <summary>Get all children.</summary>
    /// <remarks>
    /// This function exists as source for the <see cref="Nodes{T}"/>.
    /// With this construction, we can expose all children as collection.
    /// </remarks>
    public Nodes<Node> Children() => children ??= Children<Node>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Nodes<Node>? children;

    IEnumerable<XmlAnalysisNode> XmlAnalysisNode.Children() => Children();
}
