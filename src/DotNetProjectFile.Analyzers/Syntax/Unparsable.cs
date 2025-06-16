using DotNetProjectFile.Collections;
using DotNetProjectFile.Ini.Legacy;

namespace DotNetProjectFile.Syntax;

public class Unparsable : FormatException
{
    public Unparsable() { }

    public Unparsable(string message) : base(message) { }

    public Unparsable(string message, Exception innerException) : base(message, innerException) { }

    public static TSyntax Syntax<TSyntax>(TSyntax syntax, SyntaxTree tree) where TSyntax : SyntaxNode
    {
        tree = tree.With([new(tree.SourceSpan, TokenKind.UnparsableToken)]);
        var root = new RootSyntax(tree);
        var result = syntax with { Span = new SliceSpan(0, 1) };
        result.SetParent(root);
        return result;
    }
}
