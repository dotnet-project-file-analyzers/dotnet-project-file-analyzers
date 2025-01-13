using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Lexers;

[DebuggerDisplay("EOL, Kind = {Kind}")]
internal sealed class EndOfLine(string? kind) : Token(kind)
{
    /// <inheritdoc />
    [Pure]
    public override TextSpan? Match(SourceSpan source)
        => source.StartsWith("\r\n")
        ?? source.StartsWith('\n');
}
