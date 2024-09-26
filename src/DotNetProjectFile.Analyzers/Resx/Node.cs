namespace DotNetProjectFile.Resx;

public partial class Node : XmlAnalysisNode
{
    protected Node(XElement element, Resource? resource)
    {
        Element = element;
        Resource = resource ?? (this as Resource) ?? throw new ArgumentNullException(nameof(resource));
        Positions = XmlPositions.New(element);
        Depth = element.Ancestors().Count();
        Children = Element.Elements().Select(Create).OfType<Node>().ToArray();
    }

    public XElement Element { get; }

    public Resource Resource { get; }

    public int Depth { get; }

    public XmlPositions Positions { get; }

    public string LocalName => Element.Name.LocalName;

    public IReadOnlyList<Node> Children { get; }

    IEnumerable<XmlAnalysisNode> XmlAnalysisNode.Children() => Children;
}
