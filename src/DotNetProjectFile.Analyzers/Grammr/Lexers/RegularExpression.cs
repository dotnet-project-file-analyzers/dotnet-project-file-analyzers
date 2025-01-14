using Grammr.Text;
using Microsoft.CodeAnalysis.Text;
using System.Text.RegularExpressions;

namespace Grammr.Lexers;

[DebuggerDisplay("Pattern = {Pattern}, Kind = {Kind}")]
internal sealed class RegularExpression(Regex pattern, string? kind) : Token(kind)
{
    public RegularExpression(string pattern, string? kind) : this(Regex(pattern), kind) { }

    private readonly Regex Pattern = pattern;

    /// <inheritdoc />
    [Pure]
    public override TextSpan? Match(SourceSpan source) => source.Match(Pattern);

    private static Regex Regex(string regex) => regex[0] == '^'
         ? new(regex, Options, Timeout)
         : new('^' + regex, Options, Timeout);

    private static readonly RegexOptions Options = RegexOptions.CultureInvariant;

    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
}
