using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class PackageReference : Node
{
    public PackageReference(XElement element, Project project) : base(element, project) { }

    public string? Include => GetAttribute();

    public string? Version => GetAttribute();
}
