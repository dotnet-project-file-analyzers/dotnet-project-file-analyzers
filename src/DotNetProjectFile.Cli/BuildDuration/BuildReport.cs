namespace DotNetProjectFile.Cli.BuildDuration;

public sealed partial record BuildReport
{
    /// <summary>The (wall clock) duration of <c>dotnet build</c>.</summary>
    public required TimeSpan TotalDuration { get; init; }

    /// <summary>The duration of the restore, if measured.</summary>
    public TimeSpan? RestoreDuration { get; init; }

    /// <summary>The duration of the build phase (after the restore), if measured.</summary>
    public TimeSpan? BuildDuration { get; init; }

    /// <summary>The duration MSBuild reports for constructing the static project graph, if any.</summary>
    public TimeSpan? GraphBuildDuration { get; init; }

    /// <summary>The compilations of the build, per project and target framework.</summary>
    public required ImmutableArray<CompilationDuration> Compilations { get; init; }

    public int ExitCode { get; init; }

    public ImmutableArray<string> Errors { get; init; } = [];

    /// <summary>The target frameworks that have been compiled.</summary>
    public ImmutableArray<string> TargetFrameworks =>
    [
        .. Compilations
            .Select(c => c.TargetFramework)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase),
    ];

    /// <summary>The total duration of all compilations.</summary>
    public TimeSpan TotalCompilationDuration => Compilations.Select(c => c.Duration).Sum();

    /// <summary>The total execution time of all analyzers.</summary>
    public TimeSpan TotalAnalyzerDuration => Compilations.Select(c => c.TotalAnalyzerDuration).Sum();
}
