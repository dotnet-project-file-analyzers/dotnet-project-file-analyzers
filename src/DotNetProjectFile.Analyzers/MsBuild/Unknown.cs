using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class Unknown : Node
{
    public Unknown(XElement element, Project project) : base(element, project) { }

    public override string LocalName => Element.Name.LocalName;
}
