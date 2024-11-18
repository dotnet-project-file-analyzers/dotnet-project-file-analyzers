using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{Key?.Text} = {Value?.Text}")]
public sealed record KeyValuePairSyntax : IniSyntax
{
    public KeySyntax? Key => Children.OfType<KeySyntax>().FirstOrDefault();

    public ValueSyntax? Value => Children.OfType<ValueSyntax>().FirstOrDefault();

    public KeyValuePair<string, string>? Kvp
        => Key is { } key && Value is { } val
        ? new(key.Text, val.Text)
        : null;

    public bool HasAssign => Tokens.Any(t => t.Kind == TokenKind.EqualsToken || t.Kind == TokenKind.ColonToken);

    internal static IniSyntax New(Parser parser)
    {
        var root = IniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Span = kvp.Span + parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
