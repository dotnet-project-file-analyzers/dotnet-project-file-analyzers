namespace DotNetProjectFile.Parsing.Internal;

internal sealed class Or : Grammar
{
    private readonly ImmutableArray<Grammar> Options;

    public Or(Grammar left, Grammar right) => Options =
    [
        ..left is Or l ? l.Options : [left],
        ..right is Or r ? r.Options : [right],
    ];

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
    {
        foreach (var grammar in Options)
        {
            if (grammar.Match(parser) is { State: not Matching.NoMatch } next)
            {
                return next;
            }
        }
        return Parser.NoMatch;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"( {string.Join(" | ", Options)} )";
}
