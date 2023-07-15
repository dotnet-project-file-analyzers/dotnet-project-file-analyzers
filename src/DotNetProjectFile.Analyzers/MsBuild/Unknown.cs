namespace DotNetProjectFile.MsBuild;

public sealed class Unknown : Node<string>
{
    public Unknown(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string LocalName => Element.Name.LocalName;

    public override string? Value => Element.Value;
}
