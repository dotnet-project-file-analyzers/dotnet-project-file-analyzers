using DotNetProjectFile.Cli.BuildDuration;
using System.CommandLine;

namespace DotNetProjectFile.Cli;

public static class Program
{
    public static Task<int> Main(string[] args) => Commands.App.Parse(args).InvokeAsync();

    public static class Commands
    {
        public static class BuildDuration
        {
            public static readonly Argument<FileInfo> BuildTarget = new("solution-or-project")
            {
                Description = "Solution or MSBuild project to build"
            };

            public static readonly Option<string> TargetFramework = new("--framework", "-f", "--tfm")
            {
                Description = "Target framework to build. When omitted, all target frameworks are built.",
            };

            public static readonly Option<string> Configuration = new("--configuration", "-c")
            {
                Description = "Build configuration",
                DefaultValueFactory = _ => "Release",
            };

            public static readonly Option<int?> MaxCpuCount = new("--max-cpu-count", "-m")
            {
                Description = "Maximum number of MSBuild nodes to use. When omitted, MSBuild decides. Pass 1 to build projects one at a time, which makes the durations easier to attribute and also disables parallel restore.",
            };

            public static readonly Option<bool?> GraphBuild = new("--graph-build")
            {
                Description = "Builds using the project graph (MSBuild /graphBuild). When omitted, the repository decides. Pass true or false to force it on or off.",
            };

            public static readonly Option<FileInfo> Report = new("--report")
            {
                Description = "Also writes the report to a file. Without a path, it is written next to the solution or project, as <name>.build-duration.md.",
                HelpName = "path",
                Arity = ArgumentArity.ZeroOrOne,
            };

            public static readonly Command Cmd = new Command("build-duration", "Measures the duration of a non-incremental build, and of the Roslyn analyzers and source generators it runs.")
            {
                Arguments = { BuildTarget },
                Options = { TargetFramework, Configuration, MaxCpuCount, GraphBuild, Report },
            }
            .Bind(Execute);

            private static async Task<int> Execute(ParseResult result, CancellationToken cancellationToken)
            {
                var target = result.GetValue(BuildTarget)!;
                var maxCpuCount = result.GetValue(MaxCpuCount);

                var generator = new BuildReportGenerator()
                {
                    BuildTarget = target,
                    Configuration = result.GetValue(Configuration)!,
                    TargetFramework = result.GetValue(TargetFramework),
                    MaxCpuCount = maxCpuCount,
                    GraphBuild = result.GetValue(GraphBuild),
                };

                var report = await generator.Generate(cancellationToken);
                var markdown = report.ToMarkdown();

                await Console.Out.WriteAsync(markdown);

                if (result.GetResult(Report) is not null)
                {
                    var file = ReportFile(result.GetValue(Report), target);
                    file.Directory?.Create();

                    await File.WriteAllTextAsync(file.FullName, markdown, cancellationToken);
                    await Console.Error.WriteLineAsync($"Report written to {file.FullName}");
                }

                foreach (var error in report.Errors)
                {
                    await Console.Error.WriteLineAsync(error);
                }

                return report.ExitCode;
            }

            /// <summary>Gets the specified file, or the default file next to the build target, or in the specified directory.</summary>
            [Pure]
            private static FileInfo ReportFile(FileInfo? path, FileInfo target)
            {
                var name = $"{Path.GetFileNameWithoutExtension(target.Name)}.build-duration.md";

                return path switch
                {
                    null => new FileInfo(Path.Combine(target.DirectoryName!, name)),
                    _ when Directory.Exists(path.FullName) => new FileInfo(Path.Combine(path.FullName, name)),
                    _ => path,
                };
            }
        }

        public static readonly RootCommand App = new(".NET Project File Analyzers CLI")
        {
            Subcommands = { BuildDuration.Cmd },
        };
    }
}
