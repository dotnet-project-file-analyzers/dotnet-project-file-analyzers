using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

public sealed record SectionSyntax : IniSyntax
{
    public HeaderSyntax? Header { get; init; }

    public ImmutableArray<KeyValuePairSyntax> KeyValuePairs { get; init; } = [];

    public IEnumerable<KeyValuePair<string, string>> Values() => KeyValuePairs
        .Where(kvp => kvp.Key is { } && kvp.Value is { })
        .Select(kvp => new KeyValuePair<string, string>(kvp.Key!.Text, kvp.Value!.Text));

    public static IniSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax { Sections = [new()] };

        return ini with
        {
            Sections = ini.Sections.WithLast(s => s with
            {
                Tokens = s.Tokens.AddRange(parser.Tokens),
            }),
        };
    }
}
