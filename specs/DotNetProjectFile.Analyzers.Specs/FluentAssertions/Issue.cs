using Microsoft.CodeAnalysis.Text;

namespace FluentAssertions;

internal sealed record Issue(
    string Id,
    string Message, 
    LinePositionSpan Span,
    DiagnosticSeverity Severity = DiagnosticSeverity.Warning,
    string? Path = null)
{
    public static readonly IssueComparer Comparer = new();

    public Issue(string id, string message)
        : this(id, message, DiagnosticSeverity.Warning) { }

    public Issue(string id, string message, DiagnosticSeverity severity)
        : this(id, message, new(new(0, 0), new(0, 0)), severity) { }

    public Issue WithSpan(int lineStart, int posStart, int lineEnd, int posEnd)
        => this with
        {
            Span = new(new(lineStart, posStart), new(lineEnd, posEnd)),
        };

    public Issue WithPath(string path) => this with { Path = path };

    public override string ToString()
    {
        var sb = new StringBuilder($"{Id} {Message}");
        if (Span != default)
        {
            sb.Append($" @[{Span}]");
        }
        if (Severity != DiagnosticSeverity.Warning)
        {
            sb.Append($" {Severity}");
        }
        if (Path is { })
        {
            sb.Append($" {Path}");
        }
        return sb.ToString();
    }

    public static Issue WRN(string id, string message) => new(id, message, DiagnosticSeverity.Warning);

    public static Issue ERR(string id, string message) => new(id, message, DiagnosticSeverity.Error);

    public static Issue NONE(string id, string message) => new(id, message, DiagnosticSeverity.Hidden);

    public static Issue FromDiagnostic(Diagnostic diagnostic) => new(
        diagnostic.Id, 
        diagnostic.GetMessage(),
        diagnostic.Location.GetLineSpan().Span,
        diagnostic.Severity,
        System.IO.Path.GetFileName(diagnostic.Location.GetLineSpan().Path));
}
