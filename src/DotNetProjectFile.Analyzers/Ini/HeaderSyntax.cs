using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

public sealed record HeaderSyntax : IniSyntax
{
    public SourceSpanToken Token { get; init; }

    public string Text => Token.Text;

    internal static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax();

        return ini with
        {
            Sections = ini.Sections.Add(new()
            {
                Header = new()
                {
                    Token = parser.Tokens.First(t => t.Kind == TokenKind.HeaderToken),
                    Tokens = parser.Tokens,
                },
            }),
        };
    }
}
