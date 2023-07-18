using DotNetProjectFile.MsBuild.Conversion;
using Microsoft.CodeAnalysis.Text;
using System.Runtime.CompilerServices;
using System.Xml;

namespace DotNetProjectFile.MsBuild;

/// <summary>Represents node in a MS Build project file.</summary>
public abstract class Node
{
    /// <summary>Initializes a new instance of the <see cref="Node"/> class.</summary>
    protected Node(XElement element, Node? parent, Project? project)
    {
        Element = element;
        Parent = parent;
        Project = project ?? (this as Project) ?? throw new ArgumentNullException(nameof(project));
        Children = new(element.Elements().Select(Create).OfType<Node>().ToArray());
    }

    internal readonly XElement Element;

    internal readonly Project Project;

    public Node? Parent { get; }

    public virtual object? Val => Element.Value;

    /// <summary>Gets the local name of the <see cref="Node"/>.</summary>
    public virtual string LocalName => GetType().Name;

    /// <summary>Gets the label of the node.</summary>
    public string? Label => Attribute();

    public virtual string? Condition => Attribute();

    public IEnumerable<string> Conditions()
        => AncestorsAndSelf()
        .Select(n => n.Condition)
        .OfType<string>();

    /// <summary>Get the line info.</summary>
    public IXmlLineInfo LineInfo => Element;

    public int Length => ToString().Length;

    public Location Location => location ??= GetLocation();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Location? location;

    private Location GetLocation()
    {
        var path = Project.Path.FullName;
        var linePositionSpan = LineInfo.LinePositionSpan();
        var textSpan = Project.Text.TextSpan(linePositionSpan);
        return Location.Create(path, textSpan, linePositionSpan);
    }

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
    public Nodes<Node> Children { get; }

    /// <summary>Gets the <see cref="string"/> value of a child element.</summary>
    public string? Attribute([CallerMemberName] string? propertyName = null)
        => Element.Attribute(propertyName)?.Value;

    internal Node? Create(XElement element) => NodeFactory.Create(element, this, Project);

    protected T? Convert<T>(string? value, [CallerMemberName] string? propertyName = null)
        => Converters.TryConvert<T>(value, GetType(), propertyName!);

    private static readonly TypeConverters Converters = new();
}
