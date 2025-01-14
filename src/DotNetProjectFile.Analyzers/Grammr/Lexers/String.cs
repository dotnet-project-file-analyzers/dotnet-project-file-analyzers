using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Lexers;

[DebuggerDisplay("String = {Str}, Kind = {Kind}")]
internal sealed class String(string str, string? kind) : Token(kind)
{
    private readonly string Str = str;

    /// <inheritdoc />
    [Pure]
    public override TextSpan? Match(SourceSpan source) => source.StartsWith(Str);
}
