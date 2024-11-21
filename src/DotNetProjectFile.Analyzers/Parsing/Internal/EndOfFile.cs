namespace DotNetProjectFile.Parsing.Internal;

internal sealed class EndOfFile : Grammar
{
    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => parser.State == Matching.EoF
        ? parser
        : Parser.NoMatch;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => "eof";
}
