namespace Grammr.Lexers;

internal sealed class EndOfStream() : Lexer(nameof(EndOfStream))
{
    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span)
        => span.EOS ? 0 : null;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "[eos]";
}
