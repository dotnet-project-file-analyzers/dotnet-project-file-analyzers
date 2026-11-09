namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>The measurements of compiling a project for a single target framework.</summary>
public sealed record CompilationDuration
{
    public required string Project { get; init; }

    /// <summary>The target framework, or empty if unknown.</summary>
    public required string TargetFramework { get; init; }

    /// <summary>The duration of the compiler task, including running the analyzers.</summary>
    public required TimeSpan Duration { get; init; }

    /// <summary>The total execution time of the analyzers, as reported by the compiler.</summary>
    /// <remarks>
    /// Analyzers run concurrently, so this can exceed <see cref="Duration"/>.
    /// </remarks>
    public TimeSpan TotalAnalyzerDuration { get; init; }

    public required ImmutableArray<AnalyzerDuration> Analyzers { get; init; }
}
