namespace Grammr.Lexers;

internal sealed class Line(string? kind) : Lexer(kind)
{
    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader)
    {
        if (reader.EOS) return null;

        var span = reader.Span;

        var length = span.IndexOf('\n');

        if (length is -1)
            return span.Length;
        else if (length is not 0 && span[length - 1] == '\r')
            length--;

        return length is 0 ? null : length;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "line()";
}
