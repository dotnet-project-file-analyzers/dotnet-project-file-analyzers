namespace Grammr.Lexers;

internal sealed class Ch(char val, string? kind) : Lexer(kind)
{
    private readonly char Value = val;

    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader)
        => reader.EOS || reader[0] != Value ? null : 1;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"ch({Format(Value)})";
}
