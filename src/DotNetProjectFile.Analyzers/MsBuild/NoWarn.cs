namespace DotNetProjectFile.MsBuild;

public sealed class NoWarn(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project) 
{
    public IReadOnlyList<string> RuleIds
        => Value?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];
}
