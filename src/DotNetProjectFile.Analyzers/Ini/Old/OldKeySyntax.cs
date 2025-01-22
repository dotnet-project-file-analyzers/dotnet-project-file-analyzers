using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record OldKeySyntax : OldIniSyntax
{
    public string Text => Tokens.Single(t => t.Kind == TokenKind.KeyToken).Text;

    internal static OldIniSyntax New(Parser parser)
    {
        var root = OldIniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.Add(new()
                {
                    Children = [new OldKeySyntax() { Span = parser.Span }],
                    Span = parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
