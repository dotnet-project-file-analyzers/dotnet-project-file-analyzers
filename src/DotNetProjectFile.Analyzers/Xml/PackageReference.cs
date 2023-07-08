using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class PackageReference : Node
{
    public PackageReference(XElement element, Project project) : base(element, project) { }

    public string? Include => Attribute();

    public string? Version => Attribute();
}
