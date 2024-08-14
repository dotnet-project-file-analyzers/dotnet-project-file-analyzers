using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.MsBuild;

public sealed class Project : Node
{
    private Project(IOFile path, SourceText text, Projects projects, AdditionalText? additionalText, bool isProject)
        : base(XElement.Parse(text.ToString(), LoadOptions), null, null)
    {
        Path = path;
        Text = text;
        Projects = projects;
        AdditionalText = additionalText;
        IsProject = isProject;
        Imports = Children.Typed<Import>();
        PropertyGroups = Children.NestedTyped<PropertyGroup>();
        ItemGroups = Children.NestedTyped<ItemGroup>();
        WarningPragmas = WarningPragmas.New(this);
    }

#pragma warning disable QW0011 // Define properties as immutables
    // is initialized after creation (only). Hard to accomplish otherwise.
    public MsBuildProject? DirectoryBuildProps { get; internal set; }
#pragma warning restore QW0011 // Define properties as immutables

    public bool IsDirectoryBuildProps => "Directory.Build.props".Equals(Path.Name, StringComparison.OrdinalIgnoreCase);

    public AdditionalText? AdditionalText { get; }

    public bool IsAdditional => AdditionalText is { };

    public bool IsProject { get; }

    public string? Sdk => Attribute();

    public IOFile Path { get; }

    public SourceText Text { get; }

    internal Projects Projects { get; }

    public Nodes<Import> Imports { get; }

    public Nodes<PropertyGroup> PropertyGroups { get; }

    public Nodes<ItemGroup> ItemGroups { get; }

    public WarningPragmas WarningPragmas { get; }

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
        if (DirectoryBuildProps is { })
        {
            yield return DirectoryBuildProps;
        }

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

        if (DirectoryBuildProps is { })
        {
            yield return DirectoryBuildProps;
        }
    }

    public static Project Load(IOFile file, Projects projects, bool isProject)
    {
        using var reader = file.OpenText();
        return new(
            path: file,
            text: SourceText.From(reader.ReadToEnd()),
            projects: projects,
            additionalText: null,
            isProject: isProject);
    }

    public static Project Load(AdditionalText text, Projects projects, bool isProject)
        => new(
            path: IOFile.Parse(text.Path),
            text: text.GetText()!,
            projects: projects,
            additionalText: text,
            isProject: isProject);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
