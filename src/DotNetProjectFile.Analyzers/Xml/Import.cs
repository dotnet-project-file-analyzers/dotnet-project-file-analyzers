using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Import : Node
{
    public Import(XElement element) : base(element) { }

    public string? Project => GetAttribute();
}
