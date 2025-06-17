using DotNetProjectFile.Parsing;

namespace AwesomeAssertions;

internal static class DotNetProjectFIleAwesomeAssertionsExtensions
{
    [Pure]
    public static ParserAssertions Should(this Parser parser) => new(parser);

    [Pure]
    public static DiagnosticAnalyzerAssertions Should(this DiagnosticAnalyzer analyzer) => new(analyzer);

    [Pure]
    public static DiagnosticsAssertions Should(this IEnumerable<Diagnostic> diagnostics) => new(diagnostics);
}
