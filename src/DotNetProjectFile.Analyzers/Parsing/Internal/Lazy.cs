namespace DotNetProjectFile.Parsing.Internal;

internal sealed class Lazy(Func<Grammar> factory) : Grammar
{
    private readonly Func<Grammar> Factory = factory;

    private Grammar? Grammar;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
    {
        Grammar ??= Factory();
        return Grammar.Match(parser);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Grammar?.ToString() ?? "?";
}
