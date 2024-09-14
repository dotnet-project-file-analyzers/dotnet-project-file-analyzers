using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

public sealed record ValueSyntax : IniSyntax
{
    public SourceSpanToken Token => Tokens[^1];

    public string Text => Token.Text;

    internal static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax { Sections = [new()] };

        return ini with
        {
            Sections = ini.Sections.WithLast(s => s with
            {
                KeyValuePairs = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Value = new()
                    {
                        Tokens = kvp.Tokens.AddRange(parser.Tokens),
                    },
                }),
            }),
        };
    }
}
