using DotNetProjectFile.CodeAnalysis;
using DotNetProjectFile.Xml;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Resx;

public partial class Node : Locatable
{
    protected Node(XElement element, Resource? resource)
    {
        Element = element;
        Resource = resource ?? (this as Resource) ?? throw new ArgumentNullException(nameof(resource));
        Positions = XmlPositions.New(element);
    }

    internal XElement Element { get; }

    public Resource Resource { get; }

    public XmlPositions Positions { get; }

    public Location Location => location ??= Location.Create(Resource.Path.ToString(), Resource.SourceText.TextSpan(Positions.FullSpan), Positions.FullSpan);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Location? location;

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
}
