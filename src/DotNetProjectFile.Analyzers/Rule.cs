#pragma warning disable SA1118 // Parameter should not span multiple lines: readability for descriptions.

namespace DotNetProjectFile;

public static class Rule
{
    public static DiagnosticDescriptor ProjectFileCouldNotBeLocated => New(
        id: 0001,
        title: ".NET project file could not be located",
        message: "The project file '{0}' could not be located.",
        description: "In order to make these rules work, the project file should be located.",
        tags: new[] { "Configuration" },
        category: Category.Configuration,
        severity: DiagnosticSeverity.Warning,
        isEnabled: true);

    public static DiagnosticDescriptor UpdateLegacyProject => New(
        id: 0002,
        title: "Upgrade legacy .NET project file",
        message: "Upgrade legacy .NET project file.",
        description: "In order to make these rules work, the project file should be located.",
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
            helpLinkUri: $"https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/rules/Proj{id:0000}.md");
}
