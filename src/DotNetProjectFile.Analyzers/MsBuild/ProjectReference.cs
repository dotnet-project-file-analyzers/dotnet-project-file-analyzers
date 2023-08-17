using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class ProjectReference : Node<FileInfo>
{
    public ProjectReference(XElement element, Node parent, MsBuildProject project) : base(element, parent, project) { }

    public string? Include => Attribute();

    public override FileInfo? Value => value ??= GetValue();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private FileInfo? value;

    private FileInfo? GetValue()
        => Include is { Length: > 0 }
        ? Project.Path.Directory.SelectFile(Include)
        : null;
}
