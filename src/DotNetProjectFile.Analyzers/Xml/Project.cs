using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Project : Node
{
    private Project(XElement element, FileInfo path, SourceText? sourceText, Projects projects) : base(element, null!)
    {
        Path = path;
        SourceText = sourceText;
        Projects = projects;
    }

    public FileInfo Path { get; }

    private readonly Projects Projects;

    internal readonly SourceText? SourceText;

    public Nodes<Import> Imports => GetChildren<Import>();

    public Nodes<PropertyGroup> PropertyGroups => GetChildren<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => GetChildren<ItemGroup>();

    public IEnumerable<Project> GetSelfAndAccestors()
    {
        foreach (var import in Imports)
        {
            var location = new FileInfo(System.IO.Path.Combine(Path.Directory.FullName, import.Project));
            if (Projects.TryResolve(location) is { } project)
            {
                foreach (var p in project.GetSelfAndAccestors())
                {
                    yield return p;
                }
            }
        }
        yield return this;
    }

    public static Project Load(FileInfo file, Projects projects) => new(
        XElement.Load(file.OpenRead(), LoadOptions),
        file,
        null,
        projects);

    internal static Project Load(AdditionalText text, Projects projects)
    {
        var sourcText = text.GetText()!;
        return new(
            XElement.Parse(sourcText.ToString(), LoadOptions),
            new(text.Path),
            sourcText,
            projects);
    }

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
