using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record ValueSyntax : IniSyntax
{
    public string Text => Tokens.Single(t => t.Kind == TokenKind.ValueToken).Text;

    internal static IniSyntax New(Parser parser)
    {
        var root = IniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Children = kvp.Children.Add(new ValueSyntax() { Span = parser.Span }),
                    Span = kvp.Span + parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
