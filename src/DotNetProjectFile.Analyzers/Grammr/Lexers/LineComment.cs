namespace Grammr.Lexers;

internal sealed class LineComment(string start, string? kind) : Lexer(kind)
{
    private readonly string Start = start;

    /// <inheritdoc />
    [Pure]
    public override int? Match(SourceReader reader)
    {
        var span = reader.Span;

        if (!span.StartsWith(Start)) return null;

        var length = span.IndexOf('\n');

        if (length is -1)
            return span.Length;
        else if (span[length - 1] == '\r')
            length--;

        return length;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"line_comment({Start})";
}
