namespace DotNetProjectFile.Cli.AnalyzerDurations;

public sealed partial record Report
{
    public required TimeSpan TotalDuration { get; init; }
    public required ImmutableArray<AnalyzerDuration> Analyzers { get; init; }
}
