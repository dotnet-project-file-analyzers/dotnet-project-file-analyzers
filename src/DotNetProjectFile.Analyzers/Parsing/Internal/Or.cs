namespace DotNetProjectFile.Parsing.Internal;

internal sealed class Or(Grammar left, Grammar right) : Grammar
{
    private readonly Grammar Left = left;
    private readonly Grammar Right = right;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser) => parser switch
    {
        _ when Left.Match(parser) is { State: not Matching.NoMatch } next => next,
        _ when Right.Match(parser) is { State: not Matching.NoMatch } next => next,
        _ => Parser.NoMatch,
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"( {Left} | {Right} )";
}
