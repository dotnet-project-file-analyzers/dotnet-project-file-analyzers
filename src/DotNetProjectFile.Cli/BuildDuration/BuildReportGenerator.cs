namespace DotNetProjectFile.Cli.BuildDuration;

public sealed class BuildReportGenerator
{
    private static readonly Encoding UTF8_no_BOM = new UTF8Encoding(false);

    public required FileInfo BuildTarget { get; init; }

    public required string Configuration { get; init; }

    public string? TargetFramework { get; init; }

    /// <summary>The maximum number of MSBuild nodes to use.</summary>
    public int? MaxCpuCount { get; init; }

    /// <summary>Builds using the project graph (MSBuild <c>/graphBuild</c>). When <see langword="null"/>, the repository decides.</summary>
    public bool? GraphBuild { get; init; }

    [Pure]
    public async Task<BuildReport> Generate(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var progress = new BuildProgress(BuildTarget.Name, stopwatch);
        var phases = new PhaseCollector();
        var compilations = new CompilationCollector();
        var info = ProcessStartInfo();

        // Log the command before anything else, so that the build can be reproduced.
        await Console.Error.WriteLineAsync(Command(info));

        using var process = Process.Start(info)
            ?? throw new InvalidOperationException("Could not start 'dotnet'.");

        var errors = ImmutableArray.CreateBuilder<string>();

        var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        using var stopProgress = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var showProgress = Console.IsErrorRedirected
            ? Task.CompletedTask
            : progress.Show(Console.Error, stopProgress.Token);

        try
        {
            while (await process.StandardOutput.ReadLineAsync(cancellationToken) is { } line)
            {
                progress.Observe(line);
                phases.Observe(line, stopwatch.Elapsed);
                compilations.Observe(line, stopwatch.Elapsed);

                if (line.Contains(": error ", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add(BuildOutput.Message(line));
                }
            }
        }
        finally
        {
            await stopProgress.CancelAsync();
            await showProgress;
        }

        await process.WaitForExitAsync(cancellationToken);
        stopwatch.Stop();

        var stderr = await stderrTask;

        if (process.ExitCode is not 0)
        {
            foreach (var line in stderr
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Where(l => l.Contains("error", StringComparison.OrdinalIgnoreCase)))
            {
                    errors.Add(line.Trim());
            }

            if (errors.Count is 0 && !string.IsNullOrWhiteSpace(stderr))
            {
                errors.Add(stderr.Trim());
            }

            if (errors.Count is 0)
            {
                errors.Add($"The build failed with exit code {process.ExitCode}.");
            }
        }

        return new()
        {
            TotalDuration = stopwatch.Elapsed,
            RestoreDuration = phases.Restore,
            Restores = phases.Restores,
            BuildDuration = phases.Build,
            GraphBuildDuration = phases.Graph,
            Compilations = compilations.Compilations,
            ExitCode = process.ExitCode,

            // MSBuild repeats the errors in its summary at the end of the build.
            Errors = [.. errors.Distinct(StringComparer.Ordinal)],
        };
    }

    [Pure]
    public ProcessStartInfo ProcessStartInfo()
    {
#pragma warning disable S4036 // dotnet should be globally available.
        var info = new ProcessStartInfo("dotnet")
#pragma warning restore S4036
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = UTF8_no_BOM,
            StandardErrorEncoding = UTF8_no_BOM,
            WorkingDirectory = BuildTarget.DirectoryName,
            ArgumentList =
            {
                "build", BuildTarget.FullName,
                "-c", Configuration,
                "-tl:off", 
                "--no-incremental",
                "-p:reportanalyzer=true",
                "-v:diag",
                "-nologo",
            },
        };

        if (!string.IsNullOrWhiteSpace(TargetFramework))
        {
            info.ArgumentList.Add("-f");
            info.ArgumentList.Add(TargetFramework);
        }

        if (MaxCpuCount is { })
        {
            info.ArgumentList.Add($"-m:{MaxCpuCount}");
        }

        if (MaxCpuCount == 1)
        {
            info.ArgumentList.Add("-p:RestoreDisableParallel=true");
        }

        if (GraphBuild is { } graphBuild)
        {
            info.ArgumentList.Add(graphBuild ? "-graphBuild:true" : "-graphBuild:false");
        }

        info.Environment["DOTNET_CLI_UI_LANGUAGE"] = "en";
        return info;
    }

    /// <summary>Gets the command to execute, as it can be run from a shell.</summary>
    [Pure]
    public string Command()
        => Command(ProcessStartInfo());

    [Pure]
    private static string Command(ProcessStartInfo info)
        => string.Join(' ', info.ArgumentList.Prepend(info.FileName).Select(Quote));

    [Pure]
    private static string Quote(string argument)
        => argument.Length is 0 || argument.Any(c => char.IsWhiteSpace(c) || c is '"')
        ? $"\"{argument.Replace("\"", "\\\"", StringComparison.Ordinal)}\""
        : argument;
}
