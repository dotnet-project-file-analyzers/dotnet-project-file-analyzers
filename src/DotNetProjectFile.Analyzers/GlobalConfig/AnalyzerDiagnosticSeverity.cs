using DotNetProjectFile.Ini;

namespace DotNetProjectFile.GlobalConfig;

/// <summary>Represents a 'dotnet_analyzer_diagnostic.*.severity' entry.</summary>
public sealed class AnalyzerDiagnosticSeverity(IniEntry entry)
{
    /// <summary>The entry in the INI file.</summary>
    public IniEntry Entry { get; } = entry;

    /// <inheritdoc cref="IniEntry.Key" />
    public IniKey Key => Entry.Key!;

    /// <summary>Gets the diagnostic ID.</summary>
    public string DiagnosticId => Entry.Value!.Text![27..^9];

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
        && key.IsMatchStart("dotnet_analyzer_diagnostic.")
        && key.IsMatchEnd(".severity");

}
