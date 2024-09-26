namespace DotNetProjectFile.Parsing.Internal;

internal sealed class And(Grammar left, Grammar right) : Grammar
{
    private readonly Grammar Left = left;
    private readonly Grammar Right = right;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => Left.Match(parser) is { State: not Matching.NoMatch } first
        && Right.Match(first) is { State: not Matching.NoMatch } second
        ? second
        : parser.NoMatch();

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({Left} & {Right})";
}
