namespace DotNetProjectFile.MsBuild;

public sealed class Unknown : StringValueNode
{
    public Unknown(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string LocalName => Element.Name.LocalName;
}
