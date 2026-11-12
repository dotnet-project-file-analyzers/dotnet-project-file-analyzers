using System.Collections.Generic;

namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>Measures the restore, the NuGet restores, and the build phase, based on the diagnostic output of a build.</summary>
/// <remarks>
/// <c>dotnet build</c> restores before it builds. MSBuild logs the start and end of the restore,
/// and the end of every project (build), with timestamps:
/// <code>
/// 15:10:57.976     1>Project "C:\code\qowaiv\qowaiv-core.slnx" on node 1 (Restore target(s)).
/// 15:11:01.363     1>Done Building Project "C:\code\qowaiv\qowaiv-core.slnx" (Restore target(s)).
/// 15:12:24.997  5:21>Done Building Project "C:\code\qowaiv\src\Specs\Specs.csproj" (Rebuild target(s)).
/// </code>
/// The build phase starts when the restore is done, and ends when the last project is done.
/// A single node build has no timestamps, so the elapsed time of the stopwatch is used instead.
///
/// NuGet reports the duration of restoring each project separately, like
/// <c>Restored C:\code\Qowaiv\Qowaiv.csproj (in 13,88 sec).</c>.
/// </remarks>
public sealed class PhaseCollector
{
    private readonly Dictionary<string, TimeSpan> NuGetRestores = new(StringComparer.OrdinalIgnoreCase);
    private string? RestoreTarget;
    private bool? Timestamps;
    private TimeSpan? RestoreStarted;
    private TimeSpan? RestoreFinished;
    private TimeSpan? BuildFinished;

    /// <summary>The durations NuGet reported for restoring the projects.</summary>
    public ImmutableArray<RestoreDuration> Restores =>
    [
        .. NuGetRestores
            .OrderBy(restore => restore.Key, StringComparer.OrdinalIgnoreCase)
            .Select(restore => new RestoreDuration
            {
                Project = Path.GetFileNameWithoutExtension(restore.Key),
                Duration = restore.Value,
            }),
    ];

    /// <summary>The duration of the restore phase, if measured.</summary>
    public TimeSpan? Restore => Delta(RestoreStarted, RestoreFinished);

    /// <summary>The duration of the build phase, if measured.</summary>
    public TimeSpan? Build => Delta(RestoreFinished, BuildFinished);

    /// <summary>The duration MSBuild reports for constructing the static project graph, if any.</summary>
    public TimeSpan? Graph { get; private set; }

    /// <summary>Updates the measurements based on a line of the build output.</summary>
    /// <param name="line">A line of the diagnostic build output.</param>
    /// <param name="elapsed">The time elapsed since the build started.</param>
    public void Observe(string line, TimeSpan elapsed)
    {
        if (line.Contains("Restored ", StringComparison.Ordinal))
        {
            ObserveRestored(line);
        }

        if (line.Contains("Static graph", StringComparison.Ordinal))
        {
            ObserveGraph(line);
        }

        if (!line.Contains("Project \"", StringComparison.Ordinal))
        {
            return;
        }

        var prefix = BuildOutput.Prefix().Match(line);
        var timestamp = prefix.Success ? BuildOutput.Timestamp(prefix) : null;

        // The first line with project information decides how time is measured, so that
        // timestamps and the stopwatch are never mixed within a single build.
        var at = elapsed;

        if (Timestamps ??= timestamp is not null)
        {
            if (timestamp is not { } stamp)
            {
                return;
            }
            at = stamp.ToTimeSpan();
        }

        var message = prefix.Success ? line[prefix.Length..] : line;

        if (RestoreTarget is null
            && BuildOutput.ProjectStarted().Match(message) is { Success: true } started
            && started.Groups["targets"].Value == "Restore target(s)")
        {
            RestoreTarget = started.Groups["project"].Value;
            RestoreStarted = at;
        }
        else if (RestoreTarget is not null && BuildOutput.ProjectDone().Match(message) is { Success: true } done)
        {
            if (RestoreFinished is not null)
            {
                // Done Building lines can arrive out of timestamp order, so take the latest.
                BuildFinished = BuildFinished is { } finished && finished > at ? finished : at;
            }
            else if (done.Groups["project"].Value == RestoreTarget && done.Groups["targets"].Value == "Restore target(s)")
            {
                RestoreFinished = at;
            }
        }
    }

    /// <summary>Computes the elapsed time, also across midnight.</summary>
    [Pure]
    private static TimeSpan? Delta(TimeSpan? from, TimeSpan? to)
        => from is { } start && to is { } end
        ? end >= start ? end - start : end - start + TimeSpan.FromDays(1)
        : null;

    private void ObserveGraph(string line)
    {
        if (BuildOutput.GraphLoaded().Match(BuildOutput.Message(line)) is { Success: true } loaded)
        {
            // The number is formatted with the current culture, so it can use a comma.
            Graph = TimeSpan.ParseDuration(loaded.Groups["seconds"].Value, "sec");
        }
    }

    /// <summary>Records the duration NuGet reported for restoring a project.</summary>
    private void ObserveRestored(string line)
    {
        if (BuildOutput.Restored().Match(BuildOutput.Message(line)) is { Success: true } restored)
        {
            NuGetRestores[restored.Groups["project"].Value] = TimeSpan.ParseDuration(
                restored.Groups["t"].Value,
                restored.Groups["unit"].Value);
        }
    }
}
