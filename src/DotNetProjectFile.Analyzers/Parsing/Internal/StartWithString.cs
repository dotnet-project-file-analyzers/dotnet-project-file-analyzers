namespace DotNetProjectFile.Parsing.Internal;

internal sealed class StartWithString(string str, string? kind) : Grammar
{
    private readonly string String = str;
    private readonly string? Kind = kind;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => parser.Match(s => s.StartsWith(String), Kind);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Kind is { Length: > 0 }
        ? $@"string({String}, {Kind})"
        : $@"string({String})";
}
