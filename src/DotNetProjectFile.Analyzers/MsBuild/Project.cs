using Microsoft.CodeAnalysis.Text;
using System.IO;

namespace DotNetProjectFile.MsBuild;

public sealed class Project : Node
{
    private Project(FileInfo path, SourceText text, Projects projects, bool isAdditional, bool isProject)
        : base(XElement.Parse(text.ToString(), LoadOptions), null, null)
    {
        Path = path;
        Text = text;
        Projects = projects;
        IsAdditional = isAdditional;
        IsProject = isProject;
    }

    public bool IsAdditional { get; }

    public bool IsProject { get; }

    public FileInfo Path { get; }

    public SourceText Text { get; }

    internal readonly Projects Projects;

    public Nodes<Import> Imports => Children<Import>();

    public Nodes<PropertyGroup> PropertyGroups => Children<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => Children<ItemGroup>();

    public IEnumerable<Project> ImportsAndSelf()
    {
        foreach (var import in Imports)
        {
            if (import.Value is { } project)
            {
                foreach (var p in project.ImportsAndSelf())
                {
                    yield return p;
                }
            }
        }
        yield return this;
    }

    public static Project Load(FileInfo file, Projects projects, bool isProject)
    {
        using var reader = file.OpenText();
        return new(file, SourceText.From(reader.ReadToEnd()), projects, isAdditional: false, isProject);
    }

    public static Project Load(AdditionalText text, Projects projects, bool isProject)
        => new(new(text.Path), text.GetText()!, projects, isAdditional: true, isProject);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
