using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Import : Node
{
    public Import(XElement element, Project project) : base(element, project) { }

    public string? Project => GetAttribute();
}
