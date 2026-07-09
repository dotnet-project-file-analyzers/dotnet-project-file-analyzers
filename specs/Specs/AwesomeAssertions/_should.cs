using Buildalyzer;
using Grammr;
using Grammr.Lexers;

namespace AwesomeAssertions;

internal static class Shoulds
{
    [Pure]
    public static AnalyzerResultAssertions Should(this IAnalyzerResult subject) => new(subject);

    [Pure]
    public static DiagnosticAnalyzerAssertions Should(this DiagnosticAnalyzer subject) => new(subject);

    [Pure]
    public static DiagnosticsAssertions Should(this IEnumerable<Diagnostic> subject) => new(subject);

    [Pure]
    public static LexerAssertions Should(this Lexer subject) => new(subject);

    [Pure]
    public static SourceReaderAssertions Should(this SourceReader subject) => new(subject);

    [Pure]
    public static TokenAssertions Should(this Token? subject) => new(subject);
}
