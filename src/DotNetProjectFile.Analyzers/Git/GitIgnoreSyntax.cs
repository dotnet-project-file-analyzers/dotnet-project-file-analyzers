using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Git;

public sealed record GitIgnoreSyntax : Syntax.SyntaxNode
{
    public SyntaxNodeCollection<PatternSyntax> Patterns => new(this);

    internal static GitIgnoreSyntax New(Parser parser)
    {
        var root = parser.Syntax as GitIgnoreSyntax ?? new();
        return root with
        {
            Span = new(0, parser.Tokens.Count),
        };
    }

    public static GitIgnoreSyntax Parse(Syntax.SyntaxTree tree)
        => GitIgnoreGrammar.file
            .Parse(tree.SourceText)
            .Resolve<GitIgnoreSyntax>(tree);
}
