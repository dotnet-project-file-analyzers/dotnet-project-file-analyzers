using Microsoft.CodeAnalysis.Text;

namespace Grammr;

/// <summary>Helper to resolve <see cref="LinePositionSpan"/>s.</summary>
public readonly ref struct LinePositionSpans(GrammrTree tree)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly GrammrTree Tree = tree;

    /// <summary>Gets a <see cref="LinePositionSpan"/> for the <see cref="TextSpan"/>.</summary>
    public LinePositionSpan this[TextSpan span] => Tree.SourceText.Lines.GetLinePositionSpan(span);

    /// <summary>Gets a <see cref="LinePositionSpan"/> for the <see cref="Token"/>.</summary>
    public LinePositionSpan this[Token token] => this[new TextSpan(token.Start, token.Length)];
}
