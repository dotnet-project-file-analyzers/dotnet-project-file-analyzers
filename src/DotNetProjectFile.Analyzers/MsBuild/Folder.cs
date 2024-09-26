namespace DotNetProjectFile.MsBuild;

public sealed class Folder(XElement element, Node parent, MsBuildProject project)
    : Node<IODirectory>(element, parent, project)
{
    public string? Include => Attribute();

    public override IODirectory Value
        => Include is { }
        ? Project.Path.Directory.SubDirectory(Include)
        : IODirectory.Empty;
}
