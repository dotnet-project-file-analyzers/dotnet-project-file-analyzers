using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class NuGetAudit : Node
{
    public NuGetAudit(XElement element, Project? project) : base(element, project) { }

    public bool? Value => Convert<bool?>(Element.Value);
}
