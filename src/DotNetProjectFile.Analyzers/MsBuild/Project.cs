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

    public Nodes<Import> Imports => Children.OfType<Import>();

    public Nodes<PropertyGroup> PropertyGroups => Children.OfType<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => Children.OfType<ItemGroup>();

    public TValue? Property<TValue, TNode>(Func<PropertyGroup, Nodes<TNode>> selector, TValue? @default = default)
        where TNode : Node<TValue>
    {
        return SelfAndImports()
            .Select(proj => Property(proj, selector))
            .OfType<TNode>()
            .FirstOrDefault() is { } property
                ? property.Value
                : @default;

        static TNode? Property(MsBuildProject project, Func<PropertyGroup, Nodes<TNode>> selector)
            => project.PropertyGroups
                .SelectMany(selector)
                .FirstOrDefault();
    }

    /// <summary>Loops through all imports and self.</summary>
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

    /// <summary>Loops through all imports and self reversed.</summary>
    public IEnumerable<Project> SelfAndImports()
    {
        yield return this;
        foreach (var import in Imports)
        {
            if (import.Value is { } project)
            {
                foreach (var p in project.SelfAndImports())
                {
                    yield return p;
                }
            }
        }
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
