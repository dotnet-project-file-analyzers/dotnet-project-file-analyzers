using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

public readonly struct PragmaWarning(string diagnosticId, bool disable)
{
    public string DiagnosticId { get; } = diagnosticId;

    public bool Disable { get; } = disable;

    public override string ToString()
        => Disable
        ? $"#pragma warning disable {DiagnosticId}"
        : $"#pragma resore disable {DiagnosticId}";

    public static PragmaWarning? TryParse(string? str)
        => str is { }
        && Pattern.Match(str) is { Success: true } match
            ? new(
                match.Groups[nameof(DiagnosticId)].Value,
                match.Groups[nameof(Disable)].Value == "disable")
            : null;

    private static readonly Regex Pattern = new(
        @"^ *#pragma +warning +(?<Disable>disable|restore) +(?<DiagnosticId>\w+)(\s|$)",
        RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.ExplicitCapture,
        TimeSpan.FromMilliseconds(100));
}
