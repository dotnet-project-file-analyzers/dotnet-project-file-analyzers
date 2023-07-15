namespace DotNetProjectFile.MsBuild;

public sealed class TargetFrameworks : Node<IReadOnlyList<string>>
{
    public TargetFrameworks(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override IReadOnlyList<string> Value
        => Element.Value?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
        ?? Array.Empty<string>();
}
