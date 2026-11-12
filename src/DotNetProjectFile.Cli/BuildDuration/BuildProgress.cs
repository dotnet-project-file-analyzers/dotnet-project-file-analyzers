using System.Collections.Generic;

namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>Tracks the progress of a build, based on its diagnostic output.</summary>
public sealed class BuildProgress(string buildTarget, Stopwatch stopwatch)
{
    private static readonly TimeSpan RefreshRate = TimeSpan.FromMilliseconds(100);

    private readonly string BuildTarget = buildTarget;
    private readonly Stopwatch Stopwatch = stopwatch;
    private readonly Lock Locker = new();
    private readonly List<Compilation> Compilations = [];
    private string CurrentProject = buildTarget;
    private bool Restoring;

    /// <summary>Updates the progress based on a line of the build output.</summary>
    public void Observe(string line)
    {
        if (line.Contains("Begin writing request for ", StringComparison.Ordinal))
        {
            Started(BuildOutput.Message(line));
        }
        else if (line.Contains("End reading response for ", StringComparison.Ordinal)
            || line.Contains("CompilerServer: ", StringComparison.Ordinal))
        {
            Finished(BuildOutput.Message(line));
        }
        else if (line.Contains("Project \"", StringComparison.Ordinal))
        {
            Building(line);
        }
    }

    /// <summary>Describes the current state of the build.</summary>
    [Pure]
    public string Status()
    {
        var elapsed = Stopwatch.Elapsed;

        var sb = new StringBuilder()
            .Append(elapsed.Clock())
            .Append("  ");

        lock (Locker)
        {
            if (Compilations.Count == 0)
            {
                sb.Append(Restoring ? "Restoring " : "Building ").Append(CurrentProject);
            }
            else
            {
                sb.Append("Compiling ").AppendJoin(", ", Compilations.Select(Compiling));
            }
        }

        return sb.ToString();

        string Compiling(Compilation c)
        {
            return $"{c.Name} {(elapsed - c.Started).Short()}";
        }
    }

    /// <summary>Keeps writing the status to a single line, until cancelled.</summary>
    public async Task Show(TextWriter writer, CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(RefreshRate);

        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await writer.WriteAsync($"\r{Fit(Status(), Width())}");
            }
        }
        catch (OperationCanceledException)
        {
            // The build is done.
        }
        finally
        {
            await writer.WriteAsync($"\r{new string(' ', Width())}\r");
        }
    }

    private void Started(string message)
    {
        if (BuildOutput.CompilationStarted().Match(message) is { Success: true } match)
        {
            var name = match.Groups["compilation"].Value;

            if (BuildOutput.IsUnnamedCompilation(name))
            {
                return;
            }

            lock (Locker)
            {
                Compilations.Add(new(name, Stopwatch.Elapsed));
            }
        }
    }

    private void Finished(string message)
    {
        if (BuildOutput.CompilationFinished().Match(message) is { Success: true } match)
        {
            var name = match.Groups["compilation"].Value;

            lock (Locker)
            {
                Compilations.RemoveAll(c => c.Name == name);
            }
        }
    }

    private void Building(string line)
    {
        var message = BuildOutput.Message(line);

        if (message.Contains("Restore target(s)).", StringComparison.Ordinal))
        {
            var restoring = !message.Contains("Done Building Project ", StringComparison.Ordinal);

            lock (Locker)
            {
                Restoring = restoring;
                CurrentProject = BuildTarget;
            }
        }
        else if (BuildOutput.ProjectStarted().Match(message) is { Success: true } match)
        {
            var project = Path.GetFileName(match.Groups["project"].Value);

            lock (Locker)
            {
                CurrentProject = project;
            }
        }
    }

    [Pure]
    private static string Fit(string text, int width)
        => text.Length > width
        ? string.Concat(text.AsSpan(0, width - 3), "...")
        : text.PadRight(width);

    private static int Width()
    {
        try
        {
            return Math.Max(Console.WindowWidth - 1, 20);
        }
        catch (IOException)
        {
            return 79;
        }
    }

    private readonly record struct Compilation(string Name, TimeSpan Started);
}
