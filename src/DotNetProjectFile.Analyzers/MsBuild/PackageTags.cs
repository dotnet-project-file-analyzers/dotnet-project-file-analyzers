namespace DotNetProjectFile.MsBuild;

public sealed class PackageTags : Node<string>
{
    public PackageTags(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
