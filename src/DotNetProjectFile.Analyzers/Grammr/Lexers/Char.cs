using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Lexers;

[DebuggerDisplay("Ch = {Ch}, Kind = {Kind}")]
internal sealed class Char(char ch, string? kind) : Token(kind)
{
    private readonly char Ch = ch;

    /// <inheritdoc />
    [Pure]
    public override TextSpan? Match(SourceSpan source) => source.StartsWith(Ch);
}
