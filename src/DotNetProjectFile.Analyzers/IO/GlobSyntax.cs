#pragma warning disable SA1400 // Access modifier should be declared, but we want short lines.

using DotNetProjectFile.Parsing;
using DotNetProjectFile.Syntax;
using Grm = DotNetProjectFile.Parsing.Grammar;

namespace DotNetProjectFile.IO;

internal sealed class GlobGrammar : Grm
{
    static readonly Grm single /*..*/ = ch('?') /*................................*/ + GlobSyntax.Simple(".");
    static readonly Grm stars /*...*/ = (str("**") & ~ch('*')) /*.................*/ + GlobSyntax.Simple(".*");
    static readonly Grm star /*....*/ = (ch('*') & ~ch('*')) /*...................*/ + GlobSyntax.Simple("[^/]*");
    static readonly Grm literal /*.*/ = regex(@"[^\\!?*[\]{,}]+") /*..............*/ + GlobSyntax.Literal;
    static readonly Grm seq /*.....*/ = ch('[') & ch('!').Option & literal & ch(']') + GlobSyntax.Literal;

    static readonly Grm or_part /*.*/ = Lazy(() => part);

    static readonly Grm or =
        ch('{') + GlobSyntax.StartOr
        & or_part
        & (ch(',') + GlobSyntax.Comma
        & or_part).Star
        & ch('}') + GlobSyntax.EndOr;

    static readonly Grm part =
        single
        | stars
        | star
        | seq
        | or
        | literal;

    public static readonly Grm glob = part.Plus;
}

[Inheritable]
internal record GlobSyntax : Syntax.SyntaxNode
{
    public override string ToString() => string.Concat(Children);

    public static CreateSyntaxNode Simple([StringSyntax(StringSyntaxAttribute.Regex)] string expression) => parser =>
    {
        var simple = new GlobExpressionSimpleSyntax(expression);
        var root = parser.Syntax as GlobSyntax ?? new GlobSyntax();
        return root with { Children = root.Children.Add(simple) };
    };

    public static GlobSyntax Literal(Parser parser)
    {
        var text = parser.CurrentText
            .Replace(".", @"\.")
            .Replace("^", @"\^")
            .Replace('!', '^');

        var literal = new GlobExpressionSimpleSyntax(text);
        var root = parser.Syntax as GlobSyntax ?? new GlobSyntax();
        return root with { Children = root.Children.Add(literal) };
    }

    public static GlobSyntax StartOr(Parser parser)
    {
        var root = new GlobOrExpression();
        root.SetParent(parser.Syntax);
        return root;
    }

    public static Syntax.SyntaxNode Comma(Parser parser) => parser.Syntax;

    public static Syntax.SyntaxNode EndOr(Parser parser)
    {
        var root = parser.Syntax.Parent!;
        return root with { Children = root.Children.Add(parser.Syntax) };
    }

    private sealed record GlobOrExpression : GlobSyntax
    {
        public override string ToString() => $"({string.Join("|", Children)})";
    }

    private sealed record GlobExpressionSimpleSyntax(string Regex) : GlobSyntax
    {
        public override string ToString() => Regex;
    }
}
