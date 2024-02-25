namespace DotNetProjectFile.MsBuild;

public sealed class TargetFrameworks(XElement element, Node parent, MsBuildProject project)
    : Node<IReadOnlyList<string>>(element, parent, project)
{
    public override IReadOnlyList<string> Value
        => Element.Value?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];
}
