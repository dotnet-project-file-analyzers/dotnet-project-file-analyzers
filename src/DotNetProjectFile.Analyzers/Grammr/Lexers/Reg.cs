using System.Text.RegularExpressions;

namespace Grammr.Lexers;

internal sealed class Reg(Regex pattern, string? kind) : Lexer(kind)
{
    /// <summary>To be defensive against ReDoS attacks, set a maximum timeout.</summary>
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    private const RegexOptions Options
        = RegexOptions.Compiled
        | RegexOptions.ExplicitCapture
        | RegexOptions.CultureInvariant;

    public Reg(string pattern, string? kind) : this(new Regex(pattern, Options, Timeout), kind) { }

    private readonly Regex Pattern = pattern;

    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span)
        => throw new NotSupportedException("Regex is not supported as it requires a lot of string allocations.");

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
