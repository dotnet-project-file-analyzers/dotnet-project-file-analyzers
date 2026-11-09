using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Cli.AnalyzerDurations;

public sealed partial class ReportGenerator
{
    private static readonly Encoding UTF8_no_BOM = new UTF8Encoding(false);

    public async Task<Report> Generate()
    {
        var processInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = UTF8_no_BOM,
            StandardErrorEncoding = UTF8_no_BOM,
        };

        processInfo.ArgumentList.Add("-tl:off");
        processInfo.ArgumentList.Add("-p:reportanalyzer=true");
        processInfo.ArgumentList.Add("-v:diag");

        processInfo.Environment["DOTNET_CLI_UI_LANGUAGE"] = "en";

        using var process = Process.Start(processInfo)
            ?? throw new InvalidOperationException("Could not start 'dotnet'.");

        while(await process.StandardOutput.ReadLineAsync() is { } line)
        {
            if (Pattern.End().Match(line) is { Success: true } end)
            {

            }
            else if (Pattern.Total().Match(line) is { Success: true } total)
            {

            }
            else if (Pattern.Entry().Match(line) is { Success: true } entry)
            {

            }
        }

        return new()
        {
            TotalDuration = TimeSpan.Zero,
            Analyzers = [],
        };
    }

    public static partial class Pattern
    {
        [GeneratedRegex(@"^\s+(?<t><[0-9.]+|\d+(?:\.\d+)?)\s+(?<p><1|\d+)\s+(?<name>\S.*?)\s+\(TaskId:(?<id>\d+)\)$", RegexOptions.CultureInvariant)]
        public static partial Regex Entry();

        [GeneratedRegex(@"^\s*Total analyzer execution time: (?<t>[0-9.]+) seconds\. \(TaskId:(?<id>\d+)\)$", RegexOptions.CultureInvariant)]
        public static partial Regex Total();

        [GeneratedRegex(@"^\s*End reading response for (?<name>.+?) \((?<tfm>[^)]+)\) \(TaskId:(?<id>\d+)\)$", RegexOptions.CultureInvariant)]
        public static partial Regex End();
    }
}
