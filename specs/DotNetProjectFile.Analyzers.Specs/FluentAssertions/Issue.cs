using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentAssertions;

internal sealed record Issue(
    string Id,
    string Message, 
    LinePositionSpan Span = default,
    DiagnosticSeverity Severity = DiagnosticSeverity.Warning)
{
    public Issue WithSpan(int lineStart, int posStart, int lineEnd, int posEnd)
        => this with
        {
            Span = new(new(lineStart, posStart), new(lineEnd, posEnd)),
        };

    public override string ToString() => Severity == DiagnosticSeverity.Warning
        ? $"{Id} {Message} ({Span})"
        : $"{Id} {Message} ({Span}, {Severity})";

    public static Issue FromDiagnostic(Diagnostic diagnostic)
        => new(diagnostic.Id, diagnostic.GetMessage(), diagnostic.Location.GetLineSpan().Span, diagnostic.Severity);
}
