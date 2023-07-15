using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Import : Node<Project>
{
    public Import(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public override Project? Value
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
