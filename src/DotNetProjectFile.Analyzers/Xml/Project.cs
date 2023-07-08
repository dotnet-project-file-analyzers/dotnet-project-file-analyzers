using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Project : Node
{
    private Project(FileInfo path, SourceText text, Projects projects, bool isAdditional)
        : base(XElement.Parse(text.ToString(), LoadOptions), null!)
    {
        Path = path;
        Text = text;
        Projects = projects;
        IsAdditional = isAdditional;
    }

    public bool IsAdditional { get; }

    public FileInfo Path { get; }

    public SourceText Text { get; }

    internal readonly Projects Projects;

    public Nodes<Import> Imports => Children<Import>();

    public Nodes<PropertyGroup> PropertyGroups => Children<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => Children<ItemGroup>();

    public IEnumerable<Project> AncestorsAndSelf()
    {
        foreach (var import in Imports)
        {
            if (import.Value is { } project)
            {
                foreach (var p in project.AncestorsAndSelf())
                {
                    yield return p;
                }
            }
        }
        yield return this;
    }

    public static Project Load(FileInfo file, Projects projects)
    {
        using var reader = file.OpenText();
        return new(file, SourceText.From(reader.ReadToEnd()), projects, isAdditional: false);
    }

    public static Project Load(AdditionalText text, Projects projects)
        => new(new(text.Path), text.GetText()!, projects, isAdditional: true);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
