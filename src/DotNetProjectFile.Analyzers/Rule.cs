namespace DotNetProjectFile;

public static class Rule
{
    public static DiagnosticDescriptor ProjectFileCouldNotBeLocated => New(
        id: 0001,
        title: ".NET project file could not be located",
        message: "The project file '{0}' could not be located.",
        description: "In order to make this rules work, the project file should be located.",
        tags: new[] { "Configuration" },
        category: Category.Configuration,
        severity: DiagnosticSeverity.Warning,
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
