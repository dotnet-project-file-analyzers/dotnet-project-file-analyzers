namespace DotNetProjectFile.MsBuild;

public sealed class Copyright : Node<string>
{
    public Copyright(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
