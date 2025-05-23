namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Build action node:
/// * <see cref="AdditionalFiles"/>
/// * <see cref="Compile"/>
/// * <see cref="Content"/>
/// * <see cref="EditorConfgFiles"/>
/// * <see cref="EmbeddedResource"/>
/// * <see cref="GlobalAnalyzerConfigFiles"/>
/// * <see cref="None"/>.
/// </summary>
public abstract class BuildAction(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{
    public IReadOnlyList<string> Include
        => Attribute()?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    public IReadOnlyList<string> Exclude
        => Attribute()?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    public IReadOnlyList<string> Remove
        => Attribute()?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    public IReadOnlyList<string> Update
        => Attribute()?.Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    public IEnumerable<string> IncludeAndUpdate => Include.Concat(Update);
}
