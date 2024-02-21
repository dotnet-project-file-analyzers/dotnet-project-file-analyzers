using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Import(XElement element, Node parent, MsBuildProject project)
    : Node<Project>(element, parent, project)
{
    public override MsBuildProject? Value
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
    private MsBuildProject? value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool init;

    private MsBuildProject? GetValue()
    {
        var location = new FileInfo(Path.Combine(Project.Path.Directory.FullName, Attribute("Project")));
        return Project.Projects.TryResolve(location, isProject: false);
    }
}
