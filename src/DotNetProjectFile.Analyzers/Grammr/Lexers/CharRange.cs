using Grammr.Lexers;

namespace Grammr.Lexers;

internal sealed class CharRange(uint min, uint max, string? kind) : Lexer(kind)
{
    private readonly uint Min = min;
    private readonly uint Range = max - min;

    /// <inheritdoc />
    /// <remarks>
    /// Inspired by System.char.IsBetween().
    /// <code>
    /// <![CDATA[
    /// (uint)(c - minInclusive) <= (uint)(maxInclusive - minInclusive)
    /// ]]>
    /// </code>
    /// As the range is fixed, it is precalculated.
    /// </remarks>
    [Pure]
    public override int? Match(Chars span)
        => span.EOS
        || (span[0] - Min) > Range
        ? null
        : 1;

    public override string ToString() => $"[{(char)Min}-{(char)(Min + Range)}]";
}
