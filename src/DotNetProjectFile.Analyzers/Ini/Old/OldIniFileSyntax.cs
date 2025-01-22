using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record OldIniFileSyntax : OldIniSyntax
{
    public SyntaxNodeCollection<OldSectionSyntax> Sections => new(this);

    public override IEnumerable<Diagnostic> GetDiagnostics() =>
    [
        .. Tokens
            .Where(t => t.Kind == TokenKind.UnparsableToken)
            .Select(t => Diagnostic.Create(Rule.Ini.Invalid, SyntaxTree.GetLocation(t.LinePositionSpan))),

        .. Children.SelectMany(c => c.GetDiagnostics()),
    ];

    internal static OldIniFileSyntax New(Parser parser)
        => Root(parser, init: false) with
        {
            Span = new(0, parser.Tokens.Count),
        };

    internal static OldIniFileSyntax Root(Parser parser, bool init = true)
        => parser.Syntax as OldIniFileSyntax
        ?? (init
            ? new() { Children = [new OldSectionSyntax()] }
            : new());

    public static OldIniFileSyntax Parse(Syntax.SyntaxTree tree)
    {
        try
        {
            return IniGrammar.file
                .Parse(tree.SourceText)
                .Resolve<OldIniFileSyntax>(tree);
        }
        catch (Unparsable)
        {
            return Unparsable.Syntax(new OldIniFileSyntax(), tree);
        }
    }
}
