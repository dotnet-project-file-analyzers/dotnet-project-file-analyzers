using System.Text.RegularExpressions;

namespace DotNetProjectFile.Cli.BuildDuration;

/// <summary>Describes the diagnostic console output of <c>dotnet build</c>, as far as we use it.</summary>
/// <remarks>
/// MSBuild prefixes a line when it switches to the output of another project (instance). Except
/// for <see cref="Prefix"/>, the patterns match the message of a line: the line without that prefix.
/// </remarks>
internal static partial class BuildOutput
{
    /// <summary>Gets the message of a line, without the prefix MSBuild might have added, trimmed.</summary>
    [Pure]
    public static string Message(string line)
        => (Prefix().Match(line) is { Success: true } prefix ? line[prefix.Length..] : line).Trim();

    /// <summary>Gets the timestamp of the prefix, if it has one.</summary>
    /// <remarks>
    /// As a time of day, subtracting two timestamps gives the time elapsed, also across midnight.
    /// </remarks>
    [Pure]
    public static TimeOnly? Timestamp(Match prefix) => prefix.Groups["h"].Success
        ? new TimeOnly(
            hour: int.Parse(prefix.Groups["h"].ValueSpan, CultureInfo.InvariantCulture),
            minute: int.Parse(prefix.Groups["m"].ValueSpan, CultureInfo.InvariantCulture),
            second: int.Parse(prefix.Groups["s"].ValueSpan, CultureInfo.InvariantCulture),
            millisecond: int.Parse(prefix.Groups["f"].ValueSpan, CultureInfo.InvariantCulture))
        : null;

    /// <summary>The prefix MSBuild adds when it switches to the output of another project (instance).</summary>
    /// <example><c>15:12:04.079  3:20&gt;</c>.</example>
    [GeneratedRegex(@"^(?:(?<h>\d{1,2})\D(?<m>\d{2})\D(?<s>\d{2})\D(?<f>\d{3})\s+)?(?<context>\d+(?::\d+)?)>", RegexOptions.CultureInvariant)]
    public static partial Regex Prefix();

    /// <summary>A warning or an error.</summary>
    /// <example><c>file.cs(10,38): warning QW0011: text</c> or <c>MSBUILD : error MSB1009: text</c>.</example>
    [GeneratedRegex(@": (?:warning|error)(?: [^\s:]+)? ?: ", RegexOptions.CultureInvariant)]
    public static partial Regex WarningOrError();

    /// <summary>The start of building (targets of) a project.</summary>
    /// <example>
    /// <c>Project "C:\code\qowaiv\qowaiv-core.slnx" on node 1 (Restore target(s)).</c>, or
    /// <c>Project "C:\code\qowaiv\qowaiv-core.slnx" (1) is building "C:\code\qowaiv\src\Qowaiv\Qowaiv.csproj" (4) on node 4 (_IsProjectRestoreSupported target(s)).</c>.
    /// </example>
    [GeneratedRegex(@"^\s*Project ""(?:[^""]+"" \([\d:]+\) is building "")?(?<project>[^""]+)"" (?:\([\d:]+\) )?on node \d+ \((?<targets>.+)\)\.$", RegexOptions.CultureInvariant)]
    public static partial Regex ProjectStarted();

    /// <summary>The end of building (targets of) a project.</summary>
    /// <example><c>Done Building Project "C:\code\qowaiv\qowaiv-core.slnx" (Restore target(s)).</c>.</example>
    [GeneratedRegex(@"^\s*Done Building Project ""(?<project>[^""]+)"" \((?<targets>.+)\)(?: -- FAILED)?\.$", RegexOptions.CultureInvariant)]
    public static partial Regex ProjectDone();

    /// <summary>The duration NuGet reports for restoring a project.</summary>
    /// <example>
    /// <c>Restored C:\code\Qowaiv\Qowaiv.csproj (in 13,88 sec). (TaskId:45)</c>.
    /// </example>
    /// <remarks>The number is formatted with the current culture, so it can use a comma.</remarks>
    [GeneratedRegex(@"^\s*Restored (?<project>.+?) \(in (?<t>\d+(?:[.,]\d+)?) (?<unit>ms|sec|min|hr)\)\.(?: \(TaskId:\d+\))?$", RegexOptions.CultureInvariant)]
    public static partial Regex Restored();

    /// <summary>The start of a compiler task.</summary>
    /// <example><c>Task "Csc" (TaskId:332)</c>.</example>
    [GeneratedRegex(@"^\s*Task ""(?:Csc|Vbc)"" \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex CompilerTaskStarted();

    /// <summary>The end of a compiler task.</summary>
    /// <example><c>Done executing task "Csc". (TaskId:332)</c>.</example>
    [GeneratedRegex(@"^\s*Done executing task ""(?:Csc|Vbc)""", RegexOptions.CultureInvariant)]
    public static partial Regex CompilerTaskDone();

    /// <summary>A compiler task that sends a compilation to the compiler server.</summary>
    /// <example><c>Begin writing request for Qowaiv (net10.0) (TaskId:172)</c>.</example>
    [GeneratedRegex(@"^\s*Begin writing request for (?<compilation>.+) \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex CompilationStarted();

    /// <summary>A compiler task that receives the result of a compilation.</summary>
    /// <example>
    /// <c>End reading response for Qowaiv (net10.0) (TaskId:172)</c>, or
    /// <c>CompilerServer: server - server processed compilation - Qowaiv (net10.0) (TaskId:172)</c>.
    /// </example>
    [GeneratedRegex(@"^\s*(?:End reading response for |CompilerServer: .+ - )(?<compilation>.+) \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex CompilationFinished();

    /// <summary>Gets whether a compilation is an unnamed compilation of a satellite assembly.</summary>
    /// <example><c>Unnamed compilation b2f425a6-2969-45bf-96be-a4be637accfe</c>.</example>
    /// <remarks>
    /// A negative lookahead does not work here: for the <c>CompilerServer:</c> variant, the
    /// greedy prefix would swallow <c>Unnamed compilation </c> and still match.
    /// </remarks>
    [Pure]
    public static bool IsUnnamedCompilation(string name)
        => name.StartsWith("Unnamed compilation ", StringComparison.Ordinal);

    /// <summary>The name of a compilation: the project and its target framework.</summary>
    /// <example><c>Qowaiv (net10.0)</c>.</example>
    /// <remarks>
    /// The unnamed compilations of satellite assemblies, like
    /// <c>Unnamed compilation b2f425a6-2969-45bf-96be-a4be637accfe</c>, do not match.
    /// </remarks>
    [GeneratedRegex(@"^(?<project>.+) \((?<tfm>[^()]+)\)$", RegexOptions.CultureInvariant)]
    public static partial Regex ProjectAndFramework();

    /// <summary>The duration MSBuild reports for constructing the static project graph.</summary>
    /// <example><c>Static graph loaded in 0,174 seconds: 1 nodes, 0 edges</c>.</example>
    /// <remarks>The number is formatted with the current culture, so it can use a comma.</remarks>
    [GeneratedRegex(@"^\s*Static graph loaded in (?<seconds>\d+(?:[.,]\d+)?) seconds\b", RegexOptions.CultureInvariant)]
    public static partial Regex GraphLoaded();

    /// <summary>The total execution time of the analyzers of a compilation.</summary>
    /// <example><c>Total analyzer execution time: 10.975 seconds. (TaskId:332)</c>.</example>
    [GeneratedRegex(@"^\s*Total analyzer execution time: (?<t><[0-9.]+|\d+(?:\.\d+)?) seconds\. \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex AnalyzerTotal();

    /// <summary>The total execution time of the generators of a compilation.</summary>
    /// <example><c>Total generator execution time: 2.719 seconds. (TaskId:113)</c>.</example>
    [GeneratedRegex(@"^\s*Total generator execution time: (?<t><[0-9.]+|\d+(?:\.\d+)?) seconds\. \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex GeneratorTotal();

    /// <summary>The header of the analyzers, or of the generators, of a compilation.</summary>
    /// <example><c>Time (s)    %   Analyzer (TaskId:332)</c>.</example>
    [GeneratedRegex(@"^\s*Time \(s\)\s+%\s+(?<section>Analyzer|Generator) \(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex SectionHeader();

    /// <summary>The execution time of an analyzer (assembly), or a generator.</summary>
    /// <example><c>0.293    2      SonarAnalyzer.CSharp.Rules.UnusedPrivateMember (S1144, S4487) (TaskId:332)</c>.</example>
    [GeneratedRegex(@"^\s*(?<t><[0-9.]+|\d+(?:\.\d+)?)\s+(?<p><1|\d+)\s+(?<name>\S.*?)\s+\(TaskId:\d+\)$", RegexOptions.CultureInvariant)]
    public static partial Regex TimedEntry();

    /// <summary>Gets whether a timed entry is the total of an assembly, instead of a single analyzer or generator.</summary>
    /// <example><c>System.Text.Json.SourceGeneration, Version=10.0.15.3008, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51</c>.</example>
    [Pure]
    public static bool IsAssembly(string name)
        => name.Contains(", Version=", StringComparison.Ordinal);

    /// <summary>The name of an analyzer entry, and the rules it reports, if any.</summary>
    /// <example><c>SonarAnalyzer.CSharp.Rules.UnusedPrivateMember (S1144, S4487)</c>.</example>
    [GeneratedRegex(@"^(?<analyzer>.+?)(?:\s+\((?<rules>[^()]+)\))?$", RegexOptions.CultureInvariant)]
    public static partial Regex AnalyzerAndRules();
}
