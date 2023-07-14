using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class IsPackable : Node
{
    public IsPackable(XElement element, Project? project) : base(element, project) { }

    public bool? Value => Convert<bool?>(Element.Value);
}
