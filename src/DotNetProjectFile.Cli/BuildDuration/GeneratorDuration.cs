namespace DotNetProjectFile.Cli.BuildDuration;

public sealed record GeneratorDuration
{
    public required string Generator { get; init; }

    public TimeSpan Duration { get; init; }

    /// <summary>Returns the generator and its duration.</summary>
    [Pure]
    public override string ToString() => $"{Generator}: {Duration.Format()}";
}
