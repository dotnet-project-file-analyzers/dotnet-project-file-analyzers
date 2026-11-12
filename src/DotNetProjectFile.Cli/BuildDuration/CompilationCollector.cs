using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>Collects the measurements of the compilations of a build, based on its diagnostic output.</summary>
/// <remarks>
/// MSBuild interleaves the output of the projects it builds in parallel. All lines of interest
/// end with the ID of the (compiler) task that logged them, like <c>(TaskId:332)</c>. As task
/// IDs are only unique per build node, compilations running on different nodes might share one.
/// In that case, the context of the line is decisive.
///
/// Whenever MSBuild switches to the output of another project (instance), it prefixes the line
/// with its context, like <c>4:20&gt;</c>. Lines without a prefix belong to the last context.
/// Warnings, errors, and the start of projects are the exception: they are prefixed with their
/// own context, without switching to it.
///
/// MSBuild logs asynchronously, so lines are read with a varying delay. Therefore, the duration
/// of a compilation is based on the timestamps MSBuild adds to the (always prefixed) start and
/// end of the <c>CoreCompile</c> target. The moment of reading is only used as a fallback.
/// </remarks>
public sealed class CompilationCollector
{
    private readonly List<Compilation> Running = [];
    private readonly List<(string Context, CompilationDuration Compilation)> Collected = [];
    private readonly Dictionary<string, TimeOnly> CoreCompileStarted = [];
    private readonly Dictionary<string, TimeSpan> CoreCompileDurations = [];
    private string Context = string.Empty;

    /// <summary>The compilations that have been completed.</summary>
    public ImmutableArray<CompilationDuration> Compilations =>
    [
        .. Collected.Select(c => CoreCompileDurations.TryGetValue(c.Context, out var duration)
            ? c.Compilation with { Duration = duration }
            : c.Compilation),
    ];

    /// <summary>Updates the measurements based on a line of the build output.</summary>
    public void Observe(string line, TimeSpan elapsed)
    {
        var message = line;

        if (line.Length > 0 && char.IsAsciiDigit(line[0]) && BuildOutput.Prefix().Match(line) is { Success: true } prefix)
        {
            message = line[prefix.Length..];

            if (SwitchesContext(message))
            {
                Context = prefix.Groups["context"].Value;
            }

            if (BuildOutput.Timestamp(prefix) is { } timestamp)
            {
                CoreCompile(message, prefix.Groups["context"].Value, timestamp);
            }
        }

        var taskId = TaskId(message);

        if (taskId.IsEmpty)
        {
            return;
        }

        if (message.Contains("Task \"", StringComparison.Ordinal) && BuildOutput.CompilerTaskStarted().IsMatch(message))
        {
            Running.Add(new(taskId.ToString(), Context, elapsed));
        }
        else if (Find(taskId) is { } compilation)
        {
            if (message.Contains("Done executing task \"", StringComparison.Ordinal) && BuildOutput.CompilerTaskDone().IsMatch(message))
            {
                Running.Remove(compilation);

                if (compilation.Measure(elapsed) is { } measured)
                {
                    Collected.Add((compilation.Context, measured));
                }
            }
            else
            {
                compilation.Observe(message);
            }
        }
    }

    /// <summary>Records the start and end of the <c>CoreCompile</c> target of a context.</summary>
    private void CoreCompile(string message, string context, TimeOnly timestamp)
    {
        if (message.StartsWith("Target \"CoreCompile:", StringComparison.Ordinal))
        {
            CoreCompileStarted[context] = timestamp;
        }
        else if (message.StartsWith("Done building target \"CoreCompile\"", StringComparison.Ordinal)
            && CoreCompileStarted.Remove(context, out var started))
        {
            CoreCompileDurations[context] = timestamp - started;
        }
    }

    /// <summary>Finds the running compilation with the task ID, preferring the current context.</summary>
    private Compilation? Find(ReadOnlySpan<char> taskId)
    {
        Compilation? found = null;

        foreach (var compilation in Running)
        {
            if (taskId.SequenceEqual(compilation.TaskId))
            {
                if (compilation.Context == Context)
                {
                    return compilation;
                }

                found ??= compilation;
            }
        }

        return found;
    }

    /// <summary>Gets the task ID of a line like <c>Total analyzer execution time: 10.975 seconds. (TaskId:332)</c>.</summary>
    [Pure]
    private static ReadOnlySpan<char> TaskId(string message)
    {
        if (!message.EndsWith(')'))
        {
            return [];
        }

        var start = message.LastIndexOf("(TaskId:", StringComparison.Ordinal);

        return start < 0 ? [] : message.AsSpan(start + 8, message.Length - start - 9);
    }

    [Pure]
    private static bool SwitchesContext(string message)
        => !message.StartsWith("Project \"", StringComparison.Ordinal)
        && !IsWarningOrError(message);

    [Pure]
    private static bool IsWarningOrError(string message)
        => BuildOutput.WarningOrError().IsMatch(message);

    private static AnalyzerDuration? ToAnalyzerDuration(string name, TimeSpan duration)
    {
        var match = BuildOutput.AnalyzerAndRules().Match(name.Trim());

        if (!match.Success || !match.Groups["rules"].Success)
        {
            return null;
        }

        var rules = match.Groups["rules"].Value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToImmutableArray();

        return rules.IsDefaultOrEmpty
            ? null
            : new()
            {
                Analyzer = match.Groups["analyzer"].Value.Trim(),
                RuleIds = rules,
                Duration = duration,
            };
    }

    /// <remarks>The rows of the assemblies are skipped, as they are the totals of the generators in them.</remarks>
    private static GeneratorDuration? ToGeneratorDuration(string name, TimeSpan duration)
    {
        var generator = name.Trim();

        return BuildOutput.IsAssembly(generator)
            ? null
            : new()
            {
                Generator = generator,
                Duration = duration,
            };
    }

    /// <summary>A compilation that is running.</summary>
    private sealed class Compilation(string taskId, string context, TimeSpan started)
    {
        private readonly ImmutableArray<AnalyzerDuration>.Builder Analyzers = ImmutableArray.CreateBuilder<AnalyzerDuration>();
        private readonly ImmutableArray<GeneratorDuration>.Builder Generators = ImmutableArray.CreateBuilder<GeneratorDuration>();
        private string? Name;
        private TimeSpan TotalAnalyzerDuration;
        private TimeSpan TotalGeneratorDuration;
        private MeasurementSection Section;

        public string TaskId { get; } = taskId;

        public string Context { get; } = context;

        public void Observe(string message)
        {
            if (Name is null && CompilationName(message) is { } name)
            {
                Name = name;
            }
            else if (BuildOutput.AnalyzerTotal().Match(message) is { Success: true } analyzerTotal)
            {
                TotalAnalyzerDuration += TimeSpan.ParseSeconds(analyzerTotal.Groups["t"].Value);
            }
            else if (BuildOutput.GeneratorTotal().Match(message) is { Success: true } generatorTotal)
            {
                TotalGeneratorDuration += TimeSpan.ParseSeconds(generatorTotal.Groups["t"].Value);
            }
            else if (BuildOutput.SectionHeader().Match(message) is { Success: true } header)
            {
                Section = header.Groups["section"].Value switch
                {
                    "Analyzer" => MeasurementSection.Analyzers,
                    "Generator" => MeasurementSection.Generators,
                    _ => MeasurementSection.None,
                };
            }
            else if (Section != MeasurementSection.None
                && BuildOutput.TimedEntry().Match(message) is { Success: true } entry)
            {
                var duration = TimeSpan.ParseSeconds(entry.Groups["t"].Value);
                var entryName = entry.Groups["name"].Value;

                if (Section == MeasurementSection.Analyzers && ToAnalyzerDuration(entryName, duration) is { } analyzer)
                {
                    Analyzers.Add(analyzer);
                }
                else if (Section == MeasurementSection.Generators && ToGeneratorDuration(entryName, duration) is { } generator)
                {
                    Generators.Add(generator);
                }
            }
        }

        /// <remarks>
        /// Compilations without a target framework, and without analyzers or generators, are
        /// ignored. These are the unnamed compilations of satellite assemblies.
        /// </remarks>
        public CompilationDuration? Measure(TimeSpan finished)
        {
            var name = Name is { } n ? BuildOutput.ProjectAndFramework().Match(n) : Match.Empty;

            return !name.Success
                && Analyzers.Count == 0
                && Generators.Count == 0
                && TotalAnalyzerDuration == TimeSpan.Zero
                && TotalGeneratorDuration == TimeSpan.Zero
                ? null
                : new()
                {
                    Project = name.Success ? name.Groups["project"].Value : Name ?? string.Empty,
                    TargetFramework = name.Success ? name.Groups["tfm"].Value : string.Empty,
                    Duration = finished - started,
                    TotalAnalyzerDuration = TotalAnalyzerDuration,
                    TotalGeneratorDuration = TotalGeneratorDuration,
                    Analyzers = Analyzers.ToImmutable(),
                    Generators = Generators.ToImmutable(),
                };
        }

        /// <summary>Gets the name of the compilation, when it is sent to, or received from, the compiler server.</summary>
        [Pure]
        private static string? CompilationName(string message)
        {
            var match = BuildOutput.CompilationStarted().Match(message);

            if (!match.Success)
            {
                match = BuildOutput.CompilationFinished().Match(message);
            }

            var name = match.Success ? match.Groups["compilation"].Value : null;

            return name is not null && !BuildOutput.IsUnnamedCompilation(name) ? name : null;
        }
    }

    /// <summary>The section of the compiler output that timed entries are read from.</summary>
    private enum MeasurementSection
    {
        None,
        Analyzers,
        Generators,
    }
}
