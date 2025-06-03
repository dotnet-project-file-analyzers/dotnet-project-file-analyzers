using DotNetProjectFile.MsBuild.Conversion;
using System.Runtime.CompilerServices;
using System.Xml;

namespace DotNetProjectFile.Slnx;

/// <summary>Represents node in a MS Build project file.</summary>
public abstract class Node : XmlAnalysisNode
{
    private static readonly NodeFactory Factory = new();

    /// <summary>Semi-colon separator char(s).</summary>
    protected static readonly char[] SemicolonSeparated = [';'];

    /// <summary>Initializes a new instance of the <see cref="Node"/> class.</summary>
    protected Node(XElement element, Node? parent, Solution? solution)
    {
        Element = element;
        Parent = parent;
        Solution = solution ?? (this as Solution) ?? throw new ArgumentNullException(nameof(solution));
        Children = [.. element.Elements().Select(Create)];
        Locations = XmlPositions.New(element).Locations(Solution);
        Depth = element.Depth();
    }

    public Solution Solution { get; }

    public XElement Element { get; }

    public Node? Parent { get; }

    public int Depth { get; }

    public XmlLocations Locations { get; }

    public virtual object? Val => Element.Value;

    /// <summary>Gets the local name of the <see cref="Node"/>.</summary>
    public virtual string LocalName => GetType().Name;

    /// <summary>Get the line info.</summary>
    public IXmlLineInfo LineInfo => Element;

    public int Length => ToString().Length;

    /// <summary>Represents the node as an <see cref="string"/>.</summary>
    /// <remarks>
    /// The <see cref="XNode.ToString()"/> of the underlying <see cref="XElement"/> is called.
    /// </remarks>
    public override string ToString() => Element.ToString();

    public IEnumerable<Node> AncestorsAndSelf()
    {
        var parent = this;

        while (parent is { })
        {
            yield return parent;
            parent = parent.Parent;
        }
    }

    /// <summary>Get all children.</summary>
    public IReadOnlyList<Node> Children { get; }

    public IEnumerable<Node> DescendantsAndSelf()
    {
        yield return this;

        foreach (var child in Children.SelectMany(n => n.DescendantsAndSelf()))
        {
            yield return child;
        }
    }

    /// <summary>Gets the <see cref="string"/> value of an attribute.</summary>
    public string? Attribute([CallerMemberName] string? propertyName = null)
        => Element.Attribute(propertyName)?.Value;

    /// <summary>Gets the <see cref="string"/> value of a child element.</summary>
    public string? Child([CallerMemberName] string? propertyName = null)
        => Element.Element(propertyName)?.Value;

    internal Node Create(XElement element) => Factory.Create(element, this, Solution);

    protected T? Convert<T>(string? value, [CallerMemberName] string? propertyName = null)
        => Converters.TryConvert<T>(value, GetType(), propertyName!);

    IEnumerable<XmlAnalysisNode> XmlAnalysisNode.Children() => Children;

    private static readonly TypeConverters Converters = new();
}
