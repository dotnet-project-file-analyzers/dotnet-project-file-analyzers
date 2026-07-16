namespace Grammr.Lexers;

internal sealed class Str(string val, string? kind) : Lexer(kind)
{
    private readonly string Value = val;

    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span)
        => span.StartsWith(Value)
        ? Value.Length
        : null;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"str({Format(Value)})";
}
