using Microsoft.CodeAnalysis.Text;

namespace Grammr;

public abstract partial class GrammrNode
{
    [Pure]
    public Diagnostic Issue(DiagnosticDescriptor rule, Token span, params object?[]? messageArgs)
        => Issue(rule, Spans[span], messageArgs);

    [Pure]
    public Diagnostic Issue(DiagnosticDescriptor rule, LinePositionSpan span, params object?[]? messageArgs)
        => Diagnostic.Create(
            rule,
            Location.Create(
                SourceTree.Path.ToString(),
                SourceTree.SourceText.TextSpan(span),
                span),
            messageArgs);
}
