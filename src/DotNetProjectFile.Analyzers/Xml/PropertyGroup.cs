using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Project project) : base(element, project) { }

    public Nodes<ImplicitUsings> ImplicitUsings => GetChildren<ImplicitUsings>();

    public NullableContextOptions? Nullable => GetNode<NullableContextOptions?>();

    public string? RootNamespace => GetNode();
}
