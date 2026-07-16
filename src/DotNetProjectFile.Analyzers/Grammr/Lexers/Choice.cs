namespace Grammr.Lexers;

internal sealed class Choice(IEnumerable<Lexer> lexers, bool isOptional = false) : Lexer(null)
{
    private readonly ImmutableArray<Lexer> Lexers = [.. lexers.SelectMany(Flatten)];

    /// <summary>Indicates if zero matches will also lead to success.</summary>
    /// <remarks>
    /// This is done so that <see cref="SourceReader.Take(Lexer)"/> still uses
    /// <see cref="Lexer.Kind"/> when matching any of the Lexers.
    /// </remarks>
    public bool IsOptional { get; } = isOptional;

    /// <summary>Gets the lexer of a specific index.</summary>
    public Lexer this[int index] => Lexers[index];

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
    public int Count => Lexers.Length;

    public override Lexer optional
        => IsOptional
        ? this
        : (field ??= new Choice(Lexers, true));

    private static IEnumerable<Lexer> Flatten(Lexer l) => l is Choice s ? s.Lexers : [l];

    /// <inheritdoc />
    public override int? Match(Chars span)
    {
        for (var i = 0; i < Lexers.Length; i++)
            if (Lexers[i].Match(span) is { } length) return length;

        return IsOptional ? 0 : null;
    }

    /// <inheritdoc />
    public override int? Match(SourceReader reader)
    {
        for (var i = 0; i < Lexers.Length; i++)
            if (Lexers[i].Match(reader) is { } length) return length;

        return IsOptional ? 0 : null;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({string.Join(" | ", Lexers)})" + (IsOptional ? "?" : string.Empty);
}
