﻿using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.MsBuild;

public sealed class Project : Node
{
    private Project(IOFile path, SourceText text, Projects projects, AdditionalText? additionalText)
        : this(path, text, XDocument.Parse(text.ToString(), LoadOptions), projects, additionalText)
    {
    }

    private Project(IOFile path, SourceText text, XDocument document, Projects projects, AdditionalText? additionalText)
        : base(document.Root, null, null)
    {
        Path = path;
        Text = text;
        Projects = projects;
        AdditionalText = additionalText;
        WarningPragmas = WarningPragmas.New(this);
    }

#pragma warning disable QW0011 // Define properties as immutables
    // is initialized after creation (only). Hard to accomplish otherwise.
    public MsBuildProject? DirectoryBuildProps { get; internal set; }

    public MsBuildProject? DirectoryPackagesProps { get; internal set; }
#pragma warning restore QW0011 // Define properties as immutables

    public AdditionalText? AdditionalText { get; }

    public bool IsAdditional => AdditionalText is { };

    public string? Sdk => Attribute();

    public IOFile Path { get; }

    public ProjectFileType FileType => Path switch
    {
        _ when Path.Extension.IsMatch(".csproj")
            || Path.Extension.IsMatch(".vbproj") => ProjectFileType.ProjectFile,
        _ when Path.Name.IsMatch("Directory.Build.props") => ProjectFileType.DirectoryBuild,
        _ when Path.Name.IsMatch("Directory.Packages.props") => ProjectFileType.DirectoryPackages,
        _ => ProjectFileType.Props,
    };

    public SourceText Text { get; }

    internal Projects Projects { get; }

    public Nodes<Import> Imports => new(Children);

    public Nodes<PropertyGroup> PropertyGroups => new(DescendantsAndSelf());

    public Nodes<ItemGroup> ItemGroups => new(DescendantsAndSelf());

    public WarningPragmas WarningPragmas { get; }

    public TValue? Property<TValue, TNode>(Func<PropertyGroup, IEnumerable<TNode>> selector, TValue? @default = default)
        where TNode : Node<TValue>
    {
        return SelfAndImports()
            .Select(proj => Property(proj, selector))
            .OfType<TNode>()
            .FirstOrDefault() is { } property
                ? property.Value
                : @default;

        static TNode? Property(MsBuildProject project, Func<PropertyGroup, IEnumerable<TNode>> selector)
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

        if (DirectoryPackagesProps is { })
        {
            yield return DirectoryPackagesProps;
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

        if (DirectoryPackagesProps is { })
        {
            yield return DirectoryPackagesProps;
        }

        if (DirectoryBuildProps is { })
        {
            yield return DirectoryBuildProps;
        }
    }

    public static Project Load(IOFile file, Projects projects)
    {
        using var reader = file.OpenText();
        return new(
            path: file,
            text: SourceText.From(reader.ReadToEnd()),
            projects: projects,
            additionalText: null);
    }

    public static Project Load(AdditionalText text, Projects projects)
        => new(
            path: IOFile.Parse(text.Path),
            text: text.GetText()!,
            projects: projects,
            additionalText: text);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
