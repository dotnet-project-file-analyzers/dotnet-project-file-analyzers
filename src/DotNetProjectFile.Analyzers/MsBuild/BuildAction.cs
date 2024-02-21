using System.IO;

namespace DotNetProjectFile.MsBuild;

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

    public IEnumerable<FileInfo> Files(IReadOnlyList<string> paths)
    {
        foreach (var path in paths)
        {
        }
        yield break;
    }
}
