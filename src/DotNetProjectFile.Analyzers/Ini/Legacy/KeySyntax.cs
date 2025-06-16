using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini.Legacy;

[DebuggerDisplay("{FullText}")]
public sealed record KeySyntax : IniSyntax
{
    public string Text => Tokens.Single(t => t.Kind == TokenKind.KeyToken).Text;

    internal static IniSyntax New(Parser parser)
    {
        var root = IniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.Add(new()
                {
                    Children = [new KeySyntax() { Span = parser.Span }],
                    Span = parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
