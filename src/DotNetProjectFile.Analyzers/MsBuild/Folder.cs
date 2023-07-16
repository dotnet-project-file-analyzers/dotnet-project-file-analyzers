using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Folder : Node<DirectoryInfo>
{
    public Folder(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Include => Attribute();

    public override DirectoryInfo? Value
        => Include is { }
        ? new(Path.Combine(Project.Path.FullName, Include))
        : null;
}
