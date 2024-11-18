using DotNetProjectFile.Parsing;
using System.Reflection.PortableExecutable;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record HeaderSyntax : IniSyntax
{
    public string Text => Tokens.SingleOrDefault(t => t.Kind == TokenKind.HeaderTextToken).Text;

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
