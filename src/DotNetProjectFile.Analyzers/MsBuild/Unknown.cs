using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

/// <summary>Represents an unknown node. So, a node that is not specified by VSDom.Projects.</summary>
public sealed class Unknown : Node
{
    /// <summary>Initializes a new instance of a <see cref="Unknown"/>.</summary>
    /// <param name="element">
    /// The corresponding <see cref="XElement"/>.
    /// </param>
    public Unknown(XElement element, Project project) : base(element, project) { }

    /// <summary>Gets the local name of the <see cref="Unknown"/>.</summary>
    public override string LocalName => Element.Name.LocalName;
}
