namespace DotNetProjectFile.Parsing.Internal;

internal sealed class Matches(Predicate<char> predicate, string? kind = null) : Grammar
{
    private readonly Predicate<char> Predicate = predicate;
    private readonly string? Kind = kind;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => parser.Match(s => s.Matches(Predicate), Kind);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Kind is { Length: > 0 }
        ? $@"match({Predicate}, {Kind})"
        : $@"match({Predicate})";
}
