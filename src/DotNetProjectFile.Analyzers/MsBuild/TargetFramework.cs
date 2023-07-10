using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework : Node
{
    public TargetFramework(XElement element, Project? project) : base(element, project) { }

    public string? Values => Element.Value;
}
