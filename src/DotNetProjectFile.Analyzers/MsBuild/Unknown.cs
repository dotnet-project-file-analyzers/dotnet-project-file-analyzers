namespace DotNetProjectFile.MsBuild;

public sealed class Unknown(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{
    public override string LocalName => Element.Name.LocalName;
}
