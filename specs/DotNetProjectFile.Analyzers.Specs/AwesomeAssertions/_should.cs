using Buildalyzer;
using DotNetProjectFile.Parsing;

namespace AwesomeAssertions;

internal static class DotNetProjectFIleAwesomeAssertionsExtensions
{
    [Pure]
    public static ParserAssertions Should(this Parser subject) => new(subject);

    [Pure]
    public static DiagnosticAnalyzerAssertions Should(this DiagnosticAnalyzer subject) => new(subject);

    [Pure]
    public static DiagnosticsAssertions Should(this IEnumerable<Diagnostic> subject) => new(subject);
    [Pure]
    public static AnalyzerResultAssertions Should(this IAnalyzerResult subject) => new(subject);

}
