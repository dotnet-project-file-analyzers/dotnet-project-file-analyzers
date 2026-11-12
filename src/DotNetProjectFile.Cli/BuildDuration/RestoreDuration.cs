namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>The duration NuGet reported for restoring a project.</summary>
public sealed record RestoreDuration
{
    public required string Project { get; init; }

    public TimeSpan Duration { get; init; }

    /// <summary>Returns the project and its duration.</summary>
    [Pure]
    public override string ToString() => $"{Project}: {Duration.Format()}";
}
