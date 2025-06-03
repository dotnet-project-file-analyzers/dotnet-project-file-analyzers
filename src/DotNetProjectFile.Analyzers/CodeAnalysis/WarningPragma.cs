using Microsoft.CodeAnalysis.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace DotNetProjectFile.CodeAnalysis;

/// <summary>Represents a #pragma warning (disable/restore) in a MS Build project file.</summary>
public readonly struct WarningPragma(string diagnosticId, bool disable, Location location)
{
    /// <summary>The diagnostic ID.</summary>
    public string DiagnosticId { get; } = diagnosticId;

    /// <summary>Indicates if the diagnostic is disabled (or restored).</summary>
    public bool IsDisabled { get; } = disable;

    /// <summary>The location of the #pragma warning.</summary>
    public Location Location { get; } = location;

    /// <inheritdoc />
    public override string ToString()
        => IsDisabled
        ? $"#pragma warning disable {DiagnosticId}@{Location.GetLineSpan().StartLinePosition}"
        : $"#pragma warning restore {DiagnosticId}@{Location.GetLineSpan().StartLinePosition}";

    /// <summary>Creates a new #pragma warning from an <see cref="XComment"/>.</summary>
    public static WarningPragma? New(XComment comment, ProjectFile project)
    {
        var pos = comment.LinePosition();
        var next = comment.NextNode?.LinePosition() ?? pos.Expand(comment.Value.Length);
        var span = new LinePositionSpan(pos, next);
        var location = Location.Create(project.Path.ToString(), project.Text.TextSpan(span), span);
        return TryParse(comment.Value, location);
    }

    /// <summary>Tries to parse a #pragma warning.</summary>
    public static WarningPragma? TryParse(string? str, Location location)
        => str is { }
        && Pattern.Match(str) is { Success: true } match
            ? new(
                match.Groups["DiagnosticId"].Value,
                match.Groups["Disable"].Value == "disable",
                location)
            : null;

    private static readonly Regex Pattern = new(
        @"^\s*#pragma +warning +(?<Disable>disable|restore) +(?<DiagnosticId>\w+)(\s|$)",
        RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture,
        TimeSpan.FromMilliseconds(100));
}
