using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class TargetFrameworks : Node
{
    public TargetFrameworks(XElement element, Project? project) : base(element, project) { }

    public IReadOnlyList<string> Values
        => Element.Value?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
        ?? Array.Empty<string>();
}
