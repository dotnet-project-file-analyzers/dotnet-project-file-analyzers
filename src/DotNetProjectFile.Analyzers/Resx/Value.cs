using System.Xml.Linq;

namespace DotNetProjectFile.Resx;

public sealed class Value : Node
{
    public Value(XElement element, Resource resource) : base(element, resource) { }

    public string? Text => Element.Value;
}
