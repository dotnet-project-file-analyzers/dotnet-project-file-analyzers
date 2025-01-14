using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Lexers;

[DebuggerDisplay("EOF")]
internal sealed class EndOfFile() : Token("EOF")
{
    /// <inheritdoc />
    public override TextSpan? Match(SourceSpan source)
        => source.IsEmpty
        ? source.Span
        : null;

    /// <inheritdoc />
    protected override bool AddZeroLengthToken => true;
}
