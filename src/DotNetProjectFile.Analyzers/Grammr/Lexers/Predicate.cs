using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Lexers;

[DebuggerDisplay("Condition = {Condition}, Kind = {Kind}")]
internal sealed class Predicate(Predicate<char> condition, string? kind) : Token(kind)
{
    private readonly Predicate<char> Condition = condition;

    /// <inheritdoc />
    [Pure]
    public override TextSpan? Match(SourceSpan source) => source.Predicate(Condition);
}
