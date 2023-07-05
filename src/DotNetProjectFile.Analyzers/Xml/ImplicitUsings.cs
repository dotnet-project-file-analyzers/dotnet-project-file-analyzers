using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class ImplicitUsings : Node
{
    public enum Kind
    {
        disable,
        enable,
        @true,
    }

    public ImplicitUsings(XElement element, Project project) : base(element, project) { }

    public Kind? Value => Convert<Kind?>(Element.Value);
}
