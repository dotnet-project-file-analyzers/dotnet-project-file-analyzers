using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using DotNetProjectFile.Xml.Conversion;

namespace DotNetProjectFile.Xml;

/// <summary>Represents node in a .NET project file.</summary>
public class Node
{
    /// <summary>Initializes a new instance of the <see cref="Node"/> class.</summary>
    protected Node(XElement element) => Element = element;

    internal XElement Element { get; }

    /// <summary>Gets the local name of the <see cref="Node"/>.</summary>
    public virtual string LocalName => GetType().Name;

    /// <summary>Gets the label of the node.</summary>
    public string? Label => GetAttribute();

    /// <summary>Get the line info.</summary>
    public IXmlLineInfo LineInfo => Element;

    /// <summary>Represents the node as an <see cref="string"/>.</summary>
    /// <remarks>
    /// The <see cref="XNode.ToString()"/> of the underlying <see cref="XElement"/> is called.
    /// </remarks>
    public override string ToString() => Element.ToString();

    /// <summary>Gets the a <see cref="Nodes{T}"/> of children.</summary>
    public Nodes<T> GetChildren<T>() where T : Node => new(this);

    /// <summary>Gets the <see cref="string"/> value of a child element.</summary>
    public string? GetAttribute([CallerMemberName] string? propertyName = null)
        => Element.Attribute(propertyName)?.Value;

    /// <summary>Gets the <see cref="string"/> value of a child element.</summary>
    public string? GetNode([CallerMemberName] string? propertyName = null)
        => Element.Element(propertyName)?.Value;

    /// <summary>Gets the value of a child element.</summary>
    public T? GetNode<T>([CallerMemberName] string? propertyName = null)
        => Convert<T>(GetNode(propertyName), propertyName);

    /// <summary>Get all children.</summary>
    /// <remarks>
    /// This function exists as source for the <see cref="Nodes{T}"/>.
    /// With this construction, we can expose all children as collection.
    /// </remarks>
    internal IEnumerable<Node> GetAllChildren()
        => Element.Elements().Select(Create).OfType<Node>();

    internal static Node? Create(XElement element)
    => element.Name.LocalName switch
    {
        null => null,
        nameof(Import) /*.................*/ => new Import(element),
        nameof(ItemGroup) /*..............*/ => new ItemGroup(element),
        nameof(PackageReference) /*.......*/ => new PackageReference(element),
        nameof(PropertyGroup) /*..........*/ => new PropertyGroup(element),
        _ => new Unknown(element),
    };

    private T? Convert<T>(string? value, string? propertyName)
        => Converters.TryConvert<T>(value, GetType(), propertyName!);

    private static readonly TypeConverters Converters = new();
}
