using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class PropertyGroup : Node
{
    public enum ImplicitUsingKind
    {
        disable,
        enable,
        @true,
    }

    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element) : base(element) { }

    public ImplicitUsingKind? ImplicitUsings => GetNode<ImplicitUsingKind?>();

    public NullableContextOptions? Nullable => GetNode<NullableContextOptions?>();

    public string? RootNamespace => GetNode();
}
