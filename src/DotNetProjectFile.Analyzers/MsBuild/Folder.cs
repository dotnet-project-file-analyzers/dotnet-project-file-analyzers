using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class Folder : Node
{
    public Folder(XElement element, Project project) : base(element, project) { }

    public string? Include => Attribute();

    public DirectoryInfo? Directory
        => Include is { }
        ? new(Path.Combine(Project.Path.FullName, Include))
        : null;
}
