using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Ini;

public sealed record IniFileSyntax : IniSyntax
{
    public ImmutableArray<SectionSyntax> Sections { get; init; } = [];

    private IniFileSyntax WithParents()
    {
        SetParent(new RootSyntax());

        foreach (var section in Sections)
        {
            section.SetParent(this);

            section.Header?.SetParent(section.Parent!);

            foreach (var kvp in section.KeyValuePairs)
            {
                kvp.SetParent(section);
                kvp.Key?.SetParent(kvp);
                kvp.Value?.SetParent(section);
            }
        }

        return this;
    }

    internal static IniFileSyntax New(Parser parser)
    {
        var ini = parser.Syntax as IniFileSyntax ?? new IniFileSyntax();

        return (ini with
        {
            Tokens = ini.Tokens.AddRange(parser.Tokens),
        })
        .WithParents();
    }

    public static IniFileSyntax Parse(SourceText text)
        => IniGrammar.file.Parse(text).Syntax
        as IniFileSyntax ?? throw new FormatException();
}
