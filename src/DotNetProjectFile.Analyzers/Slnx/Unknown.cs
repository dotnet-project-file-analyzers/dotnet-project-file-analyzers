namespace DotNetProjectFile.Slnx;

public sealed class Unknown(XElement element, Node parent, SolutionFile solution)
    : Node<string>(element, parent, solution)
{
    public override string LocalName => Element.Name.LocalName;
}
