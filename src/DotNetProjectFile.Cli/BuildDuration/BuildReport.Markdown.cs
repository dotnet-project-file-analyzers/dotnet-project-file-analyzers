using System.Collections.Generic;

namespace DotNetProjectFile.Cli.BuildDuration;

public sealed partial record BuildReport
{
    private const int MaxPadding = 80;

    /// <summary>The format of a percentage, with a non-breaking space before the sign.</summary>
    private const string PercentageFormat = "0.00" + Nbsp + "%";

    /// <summary>A percentage that is too small to show with two decimals.</summary>
    private const string NegligiblePercentage = "<0.01" + Nbsp + "%";

    /// <summary>The output of <see cref="PercentageFormat"/> for a zero percentage.</summary>
    private const string ZeroPercentage = "0.00" + Nbsp + "%";

    [Pure]
    public string ToMarkdown()
    {
        var sb = new StringBuilder();
        var frameworks = TargetFrameworks;

        sb.AppendLine("# Build duration").AppendLine();

        List<(string Name, TimeSpan Duration)> measurements =
        [
            ("Total", TotalDuration),
            ("Restore", RestoreDuration ?? TimeSpan.Zero),
        ];

        if (HasRestores)
        {
            measurements.Add(("NuGet restore", NuGetRestoreDuration));
        }

        measurements.Add(("Build", BuildDuration ?? TimeSpan.Zero));

        if (GraphBuildDuration is { } graph)
        {
            measurements.Add(("Graph build", graph));
        }

        measurements.Add(("Compilation (wall)", TotalCompilationDuration));
        measurements.Add(("Analyzers (summed)", TotalAnalyzerDuration));

        if (HasGenerators)
        {
            measurements.Add(("Generators (summed)", TotalGeneratorDuration));
        }

        Table(
            sb,
            [new("Measurement"), new("Duration", Right: true), new("%", Right: true)],
            [.. measurements.Select(m => new[]
            {
                m.Name,
                Cell(m.Duration),
                PercentageCell(m.Duration, TotalDuration),
            })]);

        var note = "Total is the duration of `dotnet build`. Besides restoring and building, it includes starting up, and evaluating the projects to restore. NuGet restore is the sum of the durations NuGet reported per project, and excludes evaluating the project graph. Depending on the number of MSBuild nodes, projects restore and compile in parallel, and analyzers and generators run concurrently, so the NuGet restore, compilation, analyzer, and generator totals can exceed the build duration.";

        if (GraphBuildDuration is not null)
        {
            note += " Graph build is the time MSBuild reported for constructing the static project graph, and is included in the build duration.";
        }

        note += " Compilation is the wall clock time of compiling a project. Analyzer and generator times are the sums of the execution time of their callbacks, accumulated over concurrent threads, and include waiting for an analyzer that is already running. So those sums can exceed the compilation time of the same project, and are not a share of it.";

        // Analyzer time is summed over concurrent threads. Once that sum approaches the wall clock of
        // the compilation itself, comparing the two invites reading a saving into it that is not there.
        var misleading = (TotalCompilationDuration > TimeSpan.Zero && TotalAnalyzerDuration >= TotalCompilationDuration)
            || Compilations.Any(c => c.Duration > TimeSpan.Zero && c.TotalAnalyzerDuration >= c.Duration);

        if (misleading)
        {
            note += " Where the analyzer sum comes close to or exceeds the compilation wall clock, do not take that difference for time saved by disabling analyzers: the sum is taken over concurrent threads, and compiling still has to start up, load its references, and emit its output.";
        }

        note += " The percentages show each row's share of the total of its table.";

        sb.AppendLine().AppendLine(note);

        if (HasRestores)
        {
            sb.AppendLine().AppendLine("## NuGet restore").AppendLine();

            Table(
                sb,
                [new("Project"), new("Duration", Right: true), new("%", Right: true)],
                [.. Restores
                    .OrderByDescending(restore => restore.Duration)
                    .ThenBy(restore => restore.Project, StringComparer.OrdinalIgnoreCase)
                    .Select(restore => new[]
                    {
                        restore.Project,
                        restore.Duration.Format(),
                        PercentageCell(restore.Duration, NuGetRestoreDuration),
                    })]);
        }

        if (frameworks.Length > 1)
        {
            sb.AppendLine().AppendLine("## Target frameworks").AppendLine();

            var columns = new List<Column>
            {
                new("Target framework"),
                new("Projects", Right: true),
                new("Compilation (wall)", Right: true),
                new("%", Right: true),
                new("Analyzers (summed)", Right: true),
            };

            if (HasGenerators)
            {
                columns.Add(new("Generators (summed)", Right: true));
            }

            Table(
                sb,
                [.. columns],
                [.. frameworks.Select(framework =>
                {
                    var compilations = For(framework).ToArray();
                    var compilation = compilations.Select(c => c.Duration).Sum();

                    var cells = new List<string>
                    {
                        Display(framework),
                        compilations.Select(c => c.Project).Distinct(StringComparer.OrdinalIgnoreCase).Count().ToString(CultureInfo.InvariantCulture),
                        compilation.Format(),
                        PercentageCell(compilation, TotalCompilationDuration),
                        Cell(compilations.Select(c => c.TotalAnalyzerDuration).Sum()),
                    };

                    if (HasGenerators)
                    {
                        cells.Add(Cell(compilations.Select(c => c.TotalGeneratorDuration).Sum()));
                    }

                    return cells.ToArray();
                })]);
        }

        sb.AppendLine().AppendLine("## Compilations").AppendLine();

        var compilationColumns = new List<Column>
        {
            new("Project"),
            new("Target framework"),
            new("Compilation (wall)", Right: true),
            new("%", Right: true),
            new("Analyzers (summed)", Right: true),
        };

        if (HasGenerators)
        {
            compilationColumns.Add(new("Generators (summed)", Right: true));
        }

        // A project can be compiled more than once for the same target framework, for
        // instance when it is referenced both directly and as an analyzer with an
        // explicit target framework.
        var compiled = Compilations
            .GroupBy(c => (Project: c.Project.ToUpperInvariant(), Framework: c.TargetFramework.ToUpperInvariant()))
            .ToDictionary(group => group.Key, group => group.Count());

        Table(
            sb,
            [.. compilationColumns],
            [.. Compilations
                .OrderByDescending(c => c.Duration)
                .ThenBy(c => c.Project, StringComparer.OrdinalIgnoreCase)
                .Select(c =>
                {
                    var count = compiled[(c.Project.ToUpperInvariant(), c.TargetFramework.ToUpperInvariant())];

                    var cells = new List<string>
                    {
                        count > 1 ? $"{c.Project} ({count}x)" : c.Project,
                        Display(c.TargetFramework),
                        c.Duration.Format(),
                        PercentageCell(c.Duration, TotalCompilationDuration),
                        Cell(c.TotalAnalyzerDuration),
                    };

                    if (HasGenerators)
                    {
                        cells.Add(Cell(c.TotalGeneratorDuration));
                    }

                    return cells.ToArray();
                })]);

        if (compiled.Values.Any(count => count > 1))
        {
            sb.AppendLine().AppendLine("A project and target framework that is compiled more than once is marked with the number of compilations, like `(2x)`. That is usually wasted work, for example when the project is referenced both directly and as an analyzer with an explicit target framework.");
        }

        sb.AppendLine().AppendLine("## Analyzers").AppendLine();

        AnalyzersTable(sb, frameworks);

        if (HasGenerators)
        {
            sb.AppendLine().AppendLine("## Generators").AppendLine();

            GeneratorsTable(sb, frameworks);
        }

        return sb.ToString();
    }

    /// <remarks>
    /// With multiple target frameworks, there is a column per target framework, and a total.
    /// </remarks>
    private void AnalyzersTable(StringBuilder sb, ImmutableArray<string> frameworks)
    {
        var perFramework = frameworks.Length > 1;

        var columns = new List<Column> { new("Analyzer"), new("Rules") };

        if (perFramework)
        {
            columns.AddRange(frameworks.Select(f => new Column(Display(f), Right: true)));
        }

        columns.Add(new(perFramework ? "Total" : "Duration", Right: true));
        columns.Add(new("%", Right: true));

        var rows = Compilations
            .SelectMany(c => c.Analyzers, (c, a) => (c.TargetFramework, Measurement: a))
            .GroupBy(x => x.Measurement.Analyzer, StringComparer.Ordinal)
            .Select(group => (Group: group, Total: group.Select(x => x.Measurement.Duration).Sum()))
            .OrderByDescending(x => x.Total)
            .ThenBy(x => x.Group.Key, StringComparer.Ordinal)
            .Select(x =>
            {
                var cells = new List<string>
                {
                    x.Group.Key,
                    string.Join(',', x.Group
                        .SelectMany(m => m.Measurement.RuleIds)
                        .Distinct(StringComparer.Ordinal)
                        .Order(StringComparer.Ordinal)),
                };

                if (perFramework)
                {
                    cells.AddRange(frameworks.Select(f => Cell(x.Group
                        .Where(m => Same(m.TargetFramework, f))
                        .Select(m => m.Measurement.Duration)
                        .Sum())));
                }

                cells.Add(x.Total.Format());
                cells.Add(PercentageCell(x.Total, TotalAnalyzerDuration));

                return cells.ToArray();
            });

        var total = new List<string> { "**Total**", string.Empty };

        if (perFramework)
        {
            total.AddRange(frameworks.Select(f => BoldCell(For(f).Select(c => c.TotalAnalyzerDuration).Sum())));
        }

        total.Add(BoldCell(TotalAnalyzerDuration));
        total.Add(BoldPercentageCell(TotalAnalyzerDuration, TotalAnalyzerDuration));

        Table(sb, [.. columns], [.. rows, [.. total]]);
    }

    /// <remarks>
    /// With multiple target frameworks, there is a column per target framework, and a total.
    /// </remarks>
    private void GeneratorsTable(StringBuilder sb, ImmutableArray<string> frameworks)
    {
        var perFramework = frameworks.Length > 1;

        var columns = new List<Column> { new("Generator") };

        if (perFramework)
        {
            columns.AddRange(frameworks.Select(f => new Column(Display(f), Right: true)));
        }

        columns.Add(new(perFramework ? "Total" : "Duration", Right: true));
        columns.Add(new("%", Right: true));

        var rows = Compilations
            .SelectMany(c => c.Generators, (c, g) => (c.TargetFramework, Measurement: g))
            .GroupBy(x => x.Measurement.Generator, StringComparer.Ordinal)
            .Select(group => (Group: group, Total: group.Select(x => x.Measurement.Duration).Sum()))
            .OrderByDescending(x => x.Total)
            .ThenBy(x => x.Group.Key, StringComparer.Ordinal)
            .Select(x =>
            {
                var cells = new List<string> { x.Group.Key };

                if (perFramework)
                {
                    cells.AddRange(frameworks.Select(f => Cell(x.Group
                        .Where(m => Same(m.TargetFramework, f))
                        .Select(m => m.Measurement.Duration)
                        .Sum())));
                }

                cells.Add(x.Total.Format());
                cells.Add(PercentageCell(x.Total, TotalGeneratorDuration));

                return cells.ToArray();
            });

        var total = new List<string> { "**Total**" };

        if (perFramework)
        {
            total.AddRange(frameworks.Select(f => BoldCell(For(f).Select(c => c.TotalGeneratorDuration).Sum())));
        }

        total.Add(BoldCell(TotalGeneratorDuration));
        total.Add(BoldPercentageCell(TotalGeneratorDuration, TotalGeneratorDuration));

        Table(sb, [.. columns], [.. rows, [.. total]]);
    }

    [Pure]
    private IEnumerable<CompilationDuration> For(string framework)
        => Compilations.Where(c => Same(c.TargetFramework, framework));

    [Pure]
    private static bool Same(string left, string right)
        => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    [Pure]
    private static string Display(string framework)
        => framework.Length == 0 ? "(unknown)" : framework;

    /// <remarks>Zero means not measured, so it is left empty.</remarks>
    [Pure]
    private static string Cell(TimeSpan duration)
        => duration == TimeSpan.Zero ? string.Empty : duration.Format();

    /// <remarks>Zero means not measured, so it is left empty.</remarks>
    [Pure]
    private static string BoldCell(TimeSpan duration)
        => Cell(duration) is { Length: > 0 } cell ? $"**{cell}**" : string.Empty;

    /// <summary>Returns the share of the total, or empty when not measured.</summary>
    [Pure]
    private static string PercentageCell(TimeSpan duration, TimeSpan total)
    {
        if (duration == TimeSpan.Zero || total == TimeSpan.Zero)
        {
            return string.Empty;
        }

        var percentage = Ratio.Between(duration, total).ToString(PercentageFormat, CultureInfo.InvariantCulture);

        return percentage == ZeroPercentage ? NegligiblePercentage : percentage;
    }

    /// <summary>Returns the share of the total, in bold, or empty when not measured.</summary>
    [Pure]
    private static string BoldPercentageCell(TimeSpan duration, TimeSpan total)
        => PercentageCell(duration, total) is { Length: > 0 } cell ? $"**{cell}**" : string.Empty;

    private static void Table(StringBuilder sb, Column[] columns, string[][] rows)
    {
        var widths = columns
            .Select((column, i) => rows
                .Select(row => row[i].Length)
                .Where(length => length <= MaxPadding)
                .Append(column.Header.Length)
                .Append(3)
                .Max())
            .ToArray();

        Row(sb, [.. columns.Select(c => c.Header)], columns, widths);

        sb.Append('|');

        for (var i = 0; i < columns.Length; i++)
        {
            sb.Append(' ')
                .Append(columns[i].Right ? new string('-', widths[i] - 1) + ':' : new string('-', widths[i]))
                .Append(" |");
        }

        sb.AppendLine();

        foreach (var row in rows)
        {
            Row(sb, row, columns, widths);
        }
    }

    private static void Row(StringBuilder sb, string[] cells, Column[] columns, int[] widths)
    {
        sb.Append('|');

        for (var i = 0; i < columns.Length; i++)
        {
            sb.Append(' ')
                .Append(columns[i].Right ? cells[i].PadLeft(widths[i]) : cells[i].PadRight(widths[i]))
                .Append(" |");
        }

        sb.AppendLine();
    }

    private readonly record struct Column(string Header, bool Right = false);
}
