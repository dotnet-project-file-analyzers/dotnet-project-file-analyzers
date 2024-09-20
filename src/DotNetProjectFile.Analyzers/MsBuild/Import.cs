namespace DotNetProjectFile.MsBuild;

public sealed class Import(XElement element, Node parent, MsBuildProject project)
    : Node<MsBuildProject>(element, parent, project)
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
        var path = Project.Path.Directory.File(Attribute("Project") ?? string.Empty);
        return Project.ProjectFiles.MsBuildProject(path);
    }
}
