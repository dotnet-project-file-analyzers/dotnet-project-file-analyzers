using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record HeaderSyntax : IniSyntax
{
    public string Text => Tokens.Single(t => t.Kind == TokenKind.HeaderToken).Text;

    internal static IniSyntax New(Parser parser)
    {
        var ini = IniFileSyntax.Root(parser, init: false);

        return ini with
        {
            Children = ini.Children.Add(new SectionSyntax()
            {
                Children = [new HeaderSyntax { Span = parser.Span }],
                Span = parser.Span,
            }),
        };
    }
}
