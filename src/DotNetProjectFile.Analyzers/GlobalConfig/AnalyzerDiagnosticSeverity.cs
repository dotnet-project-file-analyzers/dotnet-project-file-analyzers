using DotNetProjectFile.Ini;

namespace DotNetProjectFile.GlobalConfig;

/// <summary>Represents a 'dotnet_diagnostic.*.severity' entry.</summary>
public sealed class AnalyzerDiagnosticSeverity(IniEntry entry)
{
    private const string Prefix = "dotnet_diagnostic.";
    private const string Suffix = ".severity";
    private const int MinimumLength = 26; // "dotnet_diagnostic.severity".Length

    /// <summary>The entry in the INI file.</summary>
    public IniEntry Entry { get; } = entry;

    /// <inheritdoc cref="IniEntry.Key" />
    public IniKey Key => Entry.Key!;

    /// <summary>Gets the diagnostic ID.</summary>
    public string DiagnosticId => Entry.Key!.Text is { Length: > MinimumLength } text
        ? text[Prefix.Length..^Suffix.Length]
        : string.Empty;

    /// <summary>The raw string value of the entry.</summary>
    public string? Value => Entry.Value?.Text;

    /// <summary>The parsed value of the entry.</summary>
    public DiagnosticSeverityLevel? Level
        => Enum.TryParse<DiagnosticSeverityLevel>(Value, ignoreCase: true, out var level)
        ? level
        : null;

    /// <summary>Creates a typed entry.</summary>
    public static AnalyzerDiagnosticSeverity? Create(IniEntry entry)
        => Matches(entry.Key?.Text)
        ? new(entry)
        : null;

    private static bool Matches(string? key)
        => key is { }
        && key.IsMatchStart(Prefix)
        && key.IsMatchEnd(Suffix);

}
