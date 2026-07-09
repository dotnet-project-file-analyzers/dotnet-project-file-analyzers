namespace Grammr.Lexers;

internal sealed class EndOfLine() : Lexer(nameof(EndOfLine))
{
    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader) => reader switch
    {
        { EOS: true } => null,
        _ when reader.Span[0] is '\n' => 1,
        _ when reader.Span.StartsWith("\r\n") => 2,
        _ => null,
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "[eol]";
}
