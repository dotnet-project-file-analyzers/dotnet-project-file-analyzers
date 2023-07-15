namespace DotNetProjectFile.MsBuild;

public sealed class Copyright : Node
{
    public Copyright(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
