using System.Collections.Generic;

namespace DotNetProjectFile.Cli.BuildDuration;

public sealed partial record BuildReport
{
    private const int MaxPadding = 80;

    [Pure]
    public string ToMarkdown()
    {
        var sb = new StringBuilder();
        var frameworks = TargetFrameworks;

        sb.AppendLine("# Build duration").AppendLine();

        List<string[]> measurements =
        [
            ["Total", TotalDuration.Format()],
            ["Restore", RestoreDuration is { } restore ? restore.Format() : string.Empty],
        ];

        if (HasRestores)
        {
            measurements.Add(["NuGet restore", Cell(NuGetRestoreDuration)]);
        }

        measurements.Add(["Build", BuildDuration is { } build ? build.Format() : string.Empty]);

        if (GraphBuildDuration is not null)
        {
            measurements.Add(["Graph build", Cell(GraphBuildDuration.Value)]);
        }

        measurements.Add(["Compilation", Cell(TotalCompilationDuration)]);
        measurements.Add(["Analyzers", Cell(TotalAnalyzerDuration)]);

        if (HasGenerators)
        {
            measurements.Add(["Generators", Cell(TotalGeneratorDuration)]);
        }

        Table(
            sb,
            [new("Measurement"), new("Duration", Right: true)],
            [.. measurements]);

        var note = "Total is the duration of `dotnet build`. Besides restoring and building, it includes starting up, and evaluating the projects to restore. NuGet restore is the sum of the durations NuGet reported per project, and excludes evaluating the project graph. Depending on the number of MSBuild nodes, projects restore and compile in parallel, and analyzers and generators run concurrently, so the NuGet restore, compilation, analyzer, and generator totals can exceed the build duration.";

        if (GraphBuildDuration is not null)
        {
            note += " Graph build is the time MSBuild reported for constructing the static project graph, and is included in the build duration.";
        }

        sb.AppendLine().AppendLine(note);

        if (HasRestores)
        {
            sb.AppendLine().AppendLine("## NuGet restore").AppendLine();

            Table(
                sb,
                [new("Project"), new("Duration", Right: true)],
                [.. Restores
                    .OrderByDescending(restore => restore.Duration)
                    .ThenBy(restore => restore.Project, StringComparer.OrdinalIgnoreCase)
                    .Select(restore => new[] { restore.Project, restore.Duration.Format() })]);
        }

        if (frameworks.Length > 1)
        {
            sb.AppendLine().AppendLine("## Target frameworks").AppendLine();

            var columns = new List<Column>
            {
                new("Target framework"),
                new("Projects", Right: true),
                new("Compilation", Right: true),
                new("Analyzers", Right: true),
            };

            if (HasGenerators)
            {
                columns.Add(new("Generators", Right: true));
            }

            Table(
                sb,
                [.. columns],
                [.. frameworks.Select(framework =>
                {
                    var compilations = For(framework).ToArray();

                    var cells = new List<string>
                    {
                        Display(framework),
                        compilations.Length.ToString(CultureInfo.InvariantCulture),
                        compilations.Select(c => c.Duration).Sum().Format(),
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
            new("Compilation", Right: true),
            new("Analyzers", Right: true),
        };

        if (HasGenerators)
        {
            compilationColumns.Add(new("Generators", Right: true));
        }

        Table(
            sb,
            [.. compilationColumns],
            [.. Compilations
                .OrderByDescending(c => c.Duration)
                .ThenBy(c => c.Project, StringComparer.OrdinalIgnoreCase)
                .Select(c =>
                {
                    var cells = new List<string>
                    {
                        c.Project,
                        Display(c.TargetFramework),
                        c.Duration.Format(),
                        Cell(c.TotalAnalyzerDuration),
                    };

                    if (HasGenerators)
                    {
                        cells.Add(Cell(c.TotalGeneratorDuration));
                    }

                    return cells.ToArray();
                })]);

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

                return cells.ToArray();
            });

        var total = new List<string> { "**Total**", string.Empty };

        if (perFramework)
        {
            total.AddRange(frameworks.Select(f => BoldCell(For(f).Select(c => c.TotalAnalyzerDuration).Sum())));
        }

        total.Add(BoldCell(TotalAnalyzerDuration));

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

                return cells.ToArray();
            });

        var total = new List<string> { "**Total**" };

        if (perFramework)
        {
            total.AddRange(frameworks.Select(f => BoldCell(For(f).Select(c => c.TotalGeneratorDuration).Sum())));
        }

        total.Add(BoldCell(TotalGeneratorDuration));

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
