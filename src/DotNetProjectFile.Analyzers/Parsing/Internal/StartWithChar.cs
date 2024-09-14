namespace DotNetProjectFile.Parsing.Internal;

internal sealed class StartWithChar(char ch, string? kind) : Grammar
{
    private readonly char Char = ch;
    private readonly string? Kind = kind;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => parser.Match(s => s.StartsWith(Char), Kind);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Kind is { Length: > 0 }
        ? $@"ch({Char}, {Kind})"
        : $@"ch({Char})";
}
