using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

public sealed record KeySyntax : IniSyntax
{
    public SourceSpanToken Identifier => Tokens[^1];

    public string Text => Identifier.Text;

    internal static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax { Sections = [new()] };

        return ini with
        {
            Sections = ini.Sections.WithLast(s => s with
            {
                KeyValuePairs = s.KeyValuePairs.Add(new()
                {
                    Key = new() { Tokens = parser.Tokens },
                }),
            }),
        };
    }
}
