namespace DotNetProjectFile.Parsing.Internal;

internal sealed class And : Grammar
{
    private readonly ImmutableArray<Grammar> Sequence;

    public And(Grammar left, Grammar right) => Sequence =
    [
        ..left is And l ? l.Sequence : [left],
        ..right is And r ? r.Sequence : [right],
    ];

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
    {
        var next = parser;

        foreach (var grammar in Sequence)
        {
            next = grammar.Match(next);

            if (next.State == Matching.NoMatch)
            {
                return Parser.NoMatch;
            }
        }
        return next;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"( {string.Join(" & ", Sequence)} )";
}
