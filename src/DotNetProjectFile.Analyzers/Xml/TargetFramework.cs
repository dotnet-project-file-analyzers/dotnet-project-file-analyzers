using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class TargetFramework : Node
{
    public TargetFramework(XElement element, Project? project) : base(element, project) { }

    public string? Values => Element.Value;
}
