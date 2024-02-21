using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Folder(XElement element, Node parent, Project project)
    : Node<DirectoryInfo>(element, parent, project)
{
    public string? Include => Attribute();

    public override DirectoryInfo? Value
        => Include is { }
        ? new(Path.Combine(Project.Path.FullName, Include))
        : null;
}
