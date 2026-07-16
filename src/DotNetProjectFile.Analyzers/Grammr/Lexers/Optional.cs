namespace Grammr.Lexers;

internal sealed class Optional(Lexer lexer, string? kind) : Lexer(kind)
{
    private readonly Lexer Lexer = lexer;

    /// <inheritdoc />
    public override Lexer optional => this;

    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span) => Lexer.Match(span) ?? 0;

    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader) => Lexer.Match(reader) ?? 0;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Lexer}?";
}
