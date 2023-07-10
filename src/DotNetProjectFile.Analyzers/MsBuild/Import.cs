using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class Import : Node
{
    public Import(XElement element, Project project) : base(element, project) { }

    public Project? Value
    {
        get
        {
            if (!init)
            {
                value = GetValue();
                init = true;
            }
            return value;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Project? value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool init;

    private Project? GetValue()
    {
        var location = new FileInfo(Path.Combine(Project.Path.Directory.FullName, Attribute("Project")));
        return Project.Projects.TryResolve(location, isProject: false);
    }
}
