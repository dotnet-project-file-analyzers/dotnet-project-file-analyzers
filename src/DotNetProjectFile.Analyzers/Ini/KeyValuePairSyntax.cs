using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

public sealed record KeyValuePairSyntax : IniSyntax
{
    public KeySyntax? Key { get; init; }

    public ValueSyntax? Value { get; init; }

    internal static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax { Sections = [new()] };

        return ini with
        {
            Sections = ini.Sections.WithLast(s => s with
            {
                KeyValuePairs = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Tokens = kvp.Tokens.AddRange(parser.Tokens),
                }),
            }),
        };
    }
}
