namespace DotNetProjectFile.MsBuild;

public abstract class WarnBase(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{
    public IEnumerable<string> RuleIds
        => Value?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries).Select(id => id.Trim())
        ?? [];
}
