namespace Grammr.Lexers;

internal sealed class Predicates(Func<char, int, bool> predicate, string? kind) : Lexer(kind)
{
    private readonly Func<char, int, bool> Predicate = predicate;

    /// <inheritdoc />
    [Pure]
    public override int? Match(Chars span)
    {
        var length = 0;

        while (length < span.Length && Predicate(span[length], length))
            length++;

        return length is 0
            ? null
            : length;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"match({Predicate.Method.Name})";
}
