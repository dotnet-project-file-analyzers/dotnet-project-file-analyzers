using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAudit : Node
{
    public NuGetAudit(XElement element, Project? project) : base(element, project) { }

    public bool? Value => Convert<bool?>(Element.Value);
}
