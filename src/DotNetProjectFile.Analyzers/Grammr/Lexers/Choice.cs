namespace Grammr.Lexers;

internal sealed class Choice(IEnumerable<Lexer> lexers) : Lexer(null)
{
    private readonly ImmutableArray<Lexer> Lexers = [.. lexers.SelectMany(Flatten)];

    /// <summary>Gets the lexer of a specific index.</summary>
    public Lexer this[int index] => Lexers[index];

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
    public int Count => Lexers.Length;

    private static IEnumerable<Lexer> Flatten(Lexer l) => l is Choice s ? s.Lexers : [l];

    /// <inheritdoc />
    public override int? Match(SourceReader reader)
    {
        for (var i = 0; i < Lexers.Length; i++)
            if (Lexers[i].Match(reader) is { } length) return length;

        return null;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({string.Join(" | ", Lexers)})";
}
