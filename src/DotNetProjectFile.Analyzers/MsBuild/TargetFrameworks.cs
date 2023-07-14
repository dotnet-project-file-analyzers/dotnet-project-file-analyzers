using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class TargetFrameworks : Node
{
    public TargetFrameworks(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public IReadOnlyList<string> Values
        => Element.Value?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
        ?? Array.Empty<string>();
}
