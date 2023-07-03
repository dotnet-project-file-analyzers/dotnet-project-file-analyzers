using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public class Import : Node
{
    public Import(XElement element) : base(element) { }
}
