namespace DotNetProjectFile.Cli.BuildDuration;

public sealed partial record BuildReport
{
    /// <summary>The (wall clock) duration of <c>dotnet build</c>.</summary>
    public required TimeSpan TotalDuration { get; init; }

    /// <summary>The duration of the restore phase, if measured.</summary>
    public TimeSpan? RestoreDuration { get; init; }

    /// <summary>The durations NuGet reported for restoring the projects.</summary>
    public ImmutableArray<RestoreDuration> Restores { get; init; } = [];

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

    /// <summary>The total execution time of all generators.</summary>
    public TimeSpan TotalGeneratorDuration => Compilations.Select(c => c.TotalGeneratorDuration).Sum();

    /// <summary>The total duration NuGet reported for restoring the projects.</summary>
    /// <remarks>Projects restore in parallel, so this can exceed the restore phase.</remarks>
    public TimeSpan NuGetRestoreDuration => Restores.Select(r => r.Duration).Sum();

    /// <summary>Gets whether any NuGet restore durations have been measured.</summary>
    [Pure]
    public bool HasRestores => !Restores.IsDefaultOrEmpty;

    /// <summary>Gets whether any generators have been measured.</summary>
    [Pure]
    public bool HasGenerators
        => TotalGeneratorDuration != TimeSpan.Zero
        || Compilations.Any(c => !c.Generators.IsDefaultOrEmpty);
}
