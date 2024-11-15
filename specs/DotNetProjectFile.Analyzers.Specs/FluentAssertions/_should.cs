using DotNetProjectFile.Parsing;

namespace FluentAssertions;

internal static class DotNetProjectFIleFLuentAssertionsExtensions
{
    [Pure]
    public static ParserAssertions Should(this Parser parser) => new(parser);

    [Pure]
    public static DiagnosticAnalyzerAssertions Should(this DiagnosticAnalyzer analyzer) => new(analyzer);
}
