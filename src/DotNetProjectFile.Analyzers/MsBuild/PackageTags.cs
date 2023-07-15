namespace DotNetProjectFile.MsBuild;

public sealed class PackageTags : Node
{
    public PackageTags(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string Value => Element.Value;
}
