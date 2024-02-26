namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Build action node (&lt;Compile&gt;,&lt;Content&gt;, &lt;None&gt;, &lt;AdditionalFiles&gt;, &lt;EmbeddedResource&gt;).
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
}
