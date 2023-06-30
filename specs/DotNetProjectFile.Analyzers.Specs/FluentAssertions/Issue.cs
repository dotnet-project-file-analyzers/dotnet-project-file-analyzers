using Microsoft.CodeAnalysis;

namespace FluentAssertions;

internal sealed record Issue(string Id, string Message, DiagnosticSeverity Severity = DiagnosticSeverity.Warning)
{
    public override string ToString() => Severity == DiagnosticSeverity.Warning
        ? $"{Id} {Message}"
        : $"{Id} {Message} ({Severity})";

    public static Issue FromDiagnostic(Diagnostic diagnostic)
        => new(diagnostic.Id, diagnostic.GetMessage(), diagnostic.Severity);
}
