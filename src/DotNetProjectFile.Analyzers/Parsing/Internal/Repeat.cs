namespace DotNetProjectFile.Parsing.Internal;

internal sealed class Repeat(Grammar grammar, int min, int max) : Grammar
{
    private readonly Grammar Grammar = grammar;

    private readonly int Minimum = min;

    private readonly int Maximum = max;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
    {
        var i = 0;
        var prev = parser;
        var next = parser;

        while (next.State == Matching.Match
            && i <= Maximum)
        {
            prev = next;
            next = Grammar.Match(prev);
            i += next.State == Matching.NoMatch ? 0 : 1;
        }

        var inRange = (i >= Minimum && i <= Maximum)
            // We have infinite matches with length 0.
            || (next.State == Matching.EoF && Grammar.Match(next).State == Matching.EoF);

        return next.State switch
        {
            _ when !inRange => parser.NoMatch(),
            Matching.NoMatch => prev,
            _ => next,
        };
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Minimum switch
    {
        _ when Minimum == 0 && Maximum == 0 => $"~{Grammar}",
        _ when Minimum == 0 && Maximum == 1 => $"{Grammar}?",
        _ when Minimum == 0 && Maximum == int.MaxValue => $"{Grammar}*",
        _ when Minimum == 1 && Maximum == int.MaxValue => $"{Grammar}+",
        _ when Minimum == Maximum => $"{Grammar}[{Minimum}]",
        _ when Maximum == int.MaxValue => $"{Grammar}[{Minimum},]",
        _ => $"({Grammar})[{Minimum},{Maximum}]",
    };
}
