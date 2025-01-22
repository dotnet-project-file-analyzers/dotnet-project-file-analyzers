using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record OldHeaderSyntax : OldIniSyntax
{
    public string Text => Tokens.SingleOrDefault(t => t.Kind == TokenKind.HeaderTextToken).Text;

    public override IEnumerable<Diagnostic> GetDiagnostics()
    {
        var start = 0;
        var text = 0;
        var end = 0;

        foreach (var token in Tokens)
        {
            switch (token.Kind)
            {
                case TokenKind.HeaderTextToken: text++; break;

                case TokenKind.HeaderStartToken:
                    if (++start > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidHeader, SyntaxTree.GetLocation(token.LinePositionSpan), "[ is unexpected.")];
                    }
                    break;

                case TokenKind.HeaderEndToken:
                    if (++end > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidHeader, SyntaxTree.GetLocation(token.LinePositionSpan), "] is unexpected.")];
                    }
                    if (text == 0)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidHeader, SyntaxTree.GetLocation(token.LinePositionSpan), "] is unexpected.")];
                    }
                    break;
            }
        }
        return end == 0
            ? [Diagnostic.Create(Rule.Ini.InvalidHeader, SyntaxTree.GetLocation(Tokens[^1].LinePositionSpan), "] is expected.")]
            : [];
    }

    internal static OldIniSyntax New(Parser parser)
    {
        var ini = OldIniFileSyntax.Root(parser, init: false);

        return ini with
        {
            Children = ini.Children.Add(new OldSectionSyntax()
            {
                Children = [new OldHeaderSyntax { Span = parser.Span }],
                Span = parser.Span,
            }),
        };
    }
}
