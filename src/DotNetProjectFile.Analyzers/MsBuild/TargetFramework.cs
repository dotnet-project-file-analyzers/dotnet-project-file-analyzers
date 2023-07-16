namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework : Node<string>
{
    public TargetFramework(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override string? Value => Element.Value;
}
