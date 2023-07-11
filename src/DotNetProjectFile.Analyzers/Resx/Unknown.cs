using System.Xml.Linq;

namespace DotNetProjectFile.Resx;

public sealed class Unknown : Node
{
    public Unknown(XElement element, Resource? resource) : base(element, resource) { }
}
