namespace DotNetProjectFile.Cli.BuildDuration;

public sealed record AnalyzerDuration
{
    public required string Analyzer { get; init; }

    public required ImmutableArray<string> RuleIds { get; init; }

    public TimeSpan Duration { get; init; }

    /// <summary>Returns the analyzer, its rule IDs, and its duration.</summary>
    [Pure]
    public override string ToString() => $"{Analyzer} ({string.Join(',', RuleIds)}): {Duration.Format()}";
}
