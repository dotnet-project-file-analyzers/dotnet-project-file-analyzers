namespace DotNetProjectFile.MsBuild;

public sealed class Description : Node<string>
{
    public Description(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
