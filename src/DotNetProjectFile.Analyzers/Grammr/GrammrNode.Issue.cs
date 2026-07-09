using Microsoft.CodeAnalysis.Text;

namespace Grammr;

public abstract partial class GrammrNode
{
    /// <summary>Creates an <see cref="Diagnostic"/> issue on the token after the provide one.</summary>
    /// <param name="rule">
    /// The rule describing the issue.
    /// </param>
    /// <param name="token">
    /// The token before the token that has an issue.
    /// </param>
    /// <param name="messageArgs">
    /// The message arguments to enrich the message with.
    /// </param>
    [Pure]
    public IEnumerable<Diagnostic> IssueOnNext(DiagnosticDescriptor rule, Token token, params object?[]? messageArgs)
    {
        var index = Stream.IndexOf(token);

        // If the index is known, and there is a next, report on the next token.
        var next = index >= 0 && index < Stream.Count - 1
            ? Stream[index + 1]
            : token;

        return Issue(rule, Spans[next], messageArgs);
    }

    /// <summary>Creates an <see cref="Diagnostic"/> issue on the provided token.</summary>
    /// <param name="rule">
    /// The rule describing the issue.
    /// </param>
    /// <param name="token">
    /// The token to report on.
    /// </param>
    /// <param name="messageArgs">
    /// The message arguments to enrich the message with.
    /// </param>
    [Pure]
    public IEnumerable<Diagnostic> Issue(DiagnosticDescriptor rule, Token token, params object?[]? messageArgs)
        => Issue(rule, Spans[token], messageArgs);

    /// <summary>Creates an <see cref="Diagnostic"/> issue.</summary>
    /// <param name="rule">
    /// The rule describing the issue.
    /// </param>
    /// <param name="span">
    /// The position span to report on.
    /// </param>
    /// <param name="messageArgs">
    /// The message arguments to enrich the message with.
    /// </param>
    [Pure]
    public IEnumerable<Diagnostic> Issue(DiagnosticDescriptor rule, LinePositionSpan span, params object?[]? messageArgs) =>
    [
        Diagnostic.Create(
            rule,
            Location.Create(
                SourceTree.Path.ToString(),
                SourceTree.SourceText.TextSpan(span),
                span),
            messageArgs)
    ];
}
