#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static class Rule
{
    public static DiagnosticDescriptor ProjectFileCouldNotBeLocated => New(
        id: 0001,
        title: "MS Build project file could not be located",
        message: "The project file '{0}' could not be located.",
        description: "In order to make these rules work, the project file should be located.",
        tags: new[] { "Configuration" },
        category: Category.Configuration,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor UpdateLegacyProjects => New(
        id: 0002,
        title: "Upgrade legacy MS Build project files",
        message: "Upgrade legacy MS Build project file.",
        description: "MS Build legacy projects are not supported.",
        tags: new[] { "project file", "legacy", "obsolete" },
        category: Category.Obsolete,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor DefineUsingsExplicit => New(
        id: 0003,
        title: "Define usings explicit",
        message: "Define usings explicit.",
        description:
            "The included namespaces should be clear. To reduce the statements " +
            "per file, consider global using statements.",
        tags: new[] { "Configuration", "usings", "global" },
        category: Category.Clarity,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor RunNuGetSecurityAudit => New(
        id: 0004,
        title: "Run NuGet security audits automatically",
        message: "Run NuGet security audits automatically.",
        description:
            "To reduce security issues, NuGets security audit should be run on " +
            "every build automatically.",
        tags: new[] { "security", "NuGet", "vulnerability" },
        category: Category.Security,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor DefinePackageReferenceAssetsAsAttributes => New(
        id: 0005,
        title: "Define package reference assets as attributes",
        message: "Define package reference assets of '{0}' as attributes.",
        description:
            "To reduce security issues, NuGets security audit should be run on " +
            "every build automatically.",
        tags: new[] { "security", "NuGet", "vulnerability" },
        category: Category.Clarity,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor AddAdditionalFile => New(
        id: 0006,
        title: "Add additional files to improve static code analysis",
        message: "Add '{0}' to the additional files.",
        description:
            "By adding additional non-compiling files, those files become " +
            "available in the analyzer context too.",
        tags: new[] { "code analysis" },
        category: Category.CodeQuality,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor RemoveEmptyNodes => New(
        id: 0007,
        title: "Remove empty nodes",
        message: "Remove empty {0} node.",
        description: "Empty nodes only add noise, as they contain no information.",
        tags: new[] { "noise" },
        category: Category.Noise,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor RemoveFolderNodes => New(
        id: 0008,
        title: "Remove folder nodes",
        message: "Remove folder node '{0}'.",
        description:
            "Folders nodes only add noise. They are leftovers of directories " +
            "created in the IDE, without adding an actual file to it.",
        tags: new[] { "noise" },
        category: Category.Noise,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor DefineSingleTargetFramework => New(
        id: 0009,
        title: "Use the <TargetFramework> node for a single target framework",
        message: "Use the <TargetFramework> node instead.",
        description:
            "To prevent confusion, only use the <TargetFrameworks> node when " +
            "there are multiple target frameworks.",
        tags: new[] { "target framework", "confusion" },
        category: Category.Clarity,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor UseDotNetProjectFileAnalyzers => New(
        id: 1000,
        title: "Use the .NET project file analyzers",
        message: "Use the .NET project file analyzers.",
        description: "To improve the code quality of .NET project files.",
        tags: new[] { "roslyn", "analyzer", "cbproj", "vbproj" },
        category: Category.CodeQuality,
        isEnabled: true);

    public static DiagnosticDescriptor UseAnalyzersForPackages => New(
        id: 1001,
        title: "Use analyzers for packages",
        message: "Use {0} to analyze {1}.",
        description:
            "Some NuGet packages come with there own/dedicated Roslyn analyzers; " +
            "they just contain rules to improve the usage of those packages. " +
            "In order to get the best out of those NuGet packages, their " +
            "analyzer(s) should be used.",
        tags: new[] { "roslyn", "analyzer", "NuGet" },
        category: Category.CodeQuality,
        isEnabled: true);

    public static DiagnosticDescriptor UseSonarAnalyzers => New(
        id: 1003,
        title: "Use Sonar analyzers for packages",
        message: "Add {0}.",
        description: "Improve the code quality by adding Sonar's Roslyn analyzers.",
        tags: new[] { "roslyn", "analyzer", "NuGet", "Sonar" },
        category: Category.CodeQuality,
        isEnabled: true);

    public static DiagnosticDescriptor EmbedValidResourceFiles => New(
        id: 2000,
        title: "Embed valid resource files",
        message: "Resource file {0}",
        description: "A resource file should contain valid XML.",
        tags: new[] { "resx", "resources" },
        category: Category.Bug,
        isEnabled: true,
        severity: DiagnosticSeverity.Error);

    public static DiagnosticDescriptor DefineData => New(
        id: 2001,
        title: "Define data in a resource file",
        message: "Resource does not contain any data.",
        description: "A resource file without `<data>` elements is of no use.",
        tags: new[] { "resx", "resources" },
        category: Category.Noise,
        isEnabled: true);

    public static DiagnosticDescriptor SortDataAlphabetically => New(
        id: 2002,
        title: "Sort resource file values alphabetically",
        message: "Resource values should be ordered alphabetically by their names.",
        description:
            "To improve readability, and reduce the number of merge conflicts, " +
            "the `<data>` elements should be sorted based on the `@name` attribute.",
        tags: new[] { "resx", "resources" },
        category: Category.Clarity,
        isEnabled: true);

    public static DiagnosticDescriptor AddInvariantResources => New(
        id: 2003,
        title: "Add invariant resources",
        message: "Add invariant resource.",
        description:
            "To ensure that resources can be resolved, there should always be " +
            "a invariant, default resource.",
        tags: new[] { "resx", "resources", "cultureInfo" },
        category: Category.Bug,
        isEnabled: true);

#pragma warning disable S107 // Methods should not have too many parameters
    // it calls a ctor with even more arguments.
    private static DiagnosticDescriptor New(
        int id,
        string title,
        string message,
        string description,
        string[] tags,
        Category category,
        DiagnosticSeverity severity = DiagnosticSeverity.Warning,
        bool isEnabled = true)
#pragma warning restore S107 // Methods should not have too many parameters
        => new(
            id: $"Proj{id:0000}",
            title: title,
            messageFormat: message,
            customTags: tags,
            category: category.ToString(),
            defaultSeverity: severity,
            isEnabledByDefault: isEnabled,
            description: description,
            helpLinkUri: $"https://github.com/Corniel/dotnet-project-file-analyzers/blob/main/rules/Proj{id:0000}.md");
}
