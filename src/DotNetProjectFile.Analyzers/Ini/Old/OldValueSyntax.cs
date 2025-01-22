using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record OldValueSyntax : OldIniSyntax
{
    public string Text => Tokens.Single(t => t.Kind == TokenKind.ValueToken).Text;

    internal static OldIniSyntax New(Parser parser)
    {
        var root = OldIniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Children = kvp.Children.Add(new OldValueSyntax() { Span = parser.Span }),
                    Span = kvp.Span + parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
