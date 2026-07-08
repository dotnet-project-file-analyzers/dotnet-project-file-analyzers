using System.Text.RegularExpressions;

namespace Grammr.Lexers;

internal sealed class Reg(Regex pattern, string? kind) : Lexer(kind)
{
    private const RegexOptions Options
        = RegexOptions.Compiled
        | RegexOptions.ExplicitCapture
        | RegexOptions.CultureInvariant;

    public Reg(string pattern, string? kind) : this(new Regex(pattern, Options), kind) { }

    private readonly Regex Pattern = pattern;

    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader)
        => reader.Match(Pattern) is { Success: true } match && match.Index == reader.Start
        ? match.Length
        : null;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"regex({Pattern})";
}
