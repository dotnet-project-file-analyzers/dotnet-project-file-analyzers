namespace Grammr.Lexers;

internal sealed class EndOfLine() : Lexer(nameof(EndOfLine))
{
    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span) => span switch
    {
        { EOS: true } => null,
        _ when span[0] is '\n' => 1,
        _ when span.StartsWith("\r\n") => 2,
        _ => null,
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "[eol]";
}
