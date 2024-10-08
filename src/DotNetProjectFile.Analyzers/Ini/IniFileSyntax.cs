using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record IniFileSyntax : IniSyntax
{
    public SyntaxNodeCollection<SectionSyntax> Sections => new(this);

    internal static IniFileSyntax New(Parser parser)
        => Root(parser, init: false) with
        {
            Span = new(0, parser.Tokens.Count),
        };

    internal static IniFileSyntax Root(Parser parser, bool init = true)
        => parser.Syntax as IniFileSyntax
        ?? (init
            ? new() { Children = [new SectionSyntax()] }
            : new());

    public static IniFileSyntax Parse(Syntax.SyntaxTree tree)
        => IniGrammar.file
            .Parse(tree.SourceText)
            .Resolve<IniFileSyntax>(tree);
}
