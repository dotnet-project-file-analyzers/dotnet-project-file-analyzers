using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Project : Node
{
    public Project(XElement element, FileInfo location, Projects projects) : base(element)
    {
        Location = location;
        Projects = projects;
    }

    public FileInfo Location { get; }

    private readonly Projects Projects;

    public Nodes<Import> Imports => GetChildren<Import>();

    public Nodes<PropertyGroup> PropertyGroups => GetChildren<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => GetChildren<ItemGroup>();

    public IEnumerable<Project> GetProjects()
    {
        foreach (var import in Imports)
        {
            var location = new FileInfo(Path.Combine(Location.Directory.FullName, import.Project));
            if (Projects.TryResolve(location) is { } project)
            {
                foreach (var p in project.GetProjects())
                {
                    yield return p;
                }
            }
        }
        yield return this;
    }

    public static Project Load(FileInfo file, Projects projects) => new(
        XElement.Load(file.OpenRead(), LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo),
        file,
        projects);

    internal static Project Load(AdditionalText text, Projects projects) => new(
        XElement.Parse(text.GetText()?.ToString()),
        new(text.Path),
        projects);
}
