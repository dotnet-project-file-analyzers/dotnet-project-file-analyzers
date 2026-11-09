
namespace DotNetProjectFile.Cli.AnalyzerDurations;

public sealed record AnalyzerDuration
{
    public required string Analyzer { get; init; }
    public required ImmutableArray<string> RuleIds { get; init; } 
    public TimeSpan Duration { get; init; }

    /// Returns a string representation of the analyzer and its rule IDs and duration.A formatted string containing the analyzer name, comma-separated rule IDs, and duration.
    [Pure]
    public override string ToString() => $"{Analyzer} ({string.Join(',', RuleIds)}): {Duration}";

    public void TableEntry(StringBuilder sb)
        => sb.AppendLine($"| {Analyzer,-80} | {string.Join(',', RuleIds)} | {Duration.Format(),10} |");
}
