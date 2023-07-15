namespace DotNetProjectFile.MsBuild;

public sealed class Authors : Node
{
    public Authors(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
