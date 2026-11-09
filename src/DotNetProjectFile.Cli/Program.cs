using System.CommandLine;
using System.IO;
namespace DotNetProjectFile.Cli;

public static class Program
{
    public static Task<int> Main(string[] args) => Commands.App.Parse(args).InvokeAsync();

    public static class Commands
    {
        public static class AnalyzerDurations
        {
            public static readonly Argument<FileInfo> BuildTarget = new("solution-or-project")
            {
                Description = "Solution or MSBuild project to build"
            };

            public static readonly Command Cmd = new Command("analyzer-durations")
            {
                Arguments = { BuildTarget },
            }
            .Bind(Execute);

            private static void Execute(ParseResult result)
            {

            }
        }

        public static readonly RootCommand App = new(".NET Project File Analyzers CLI")
        {
            Subcommands = { AnalyzerDurations.Cmd },
        };
    }
}
