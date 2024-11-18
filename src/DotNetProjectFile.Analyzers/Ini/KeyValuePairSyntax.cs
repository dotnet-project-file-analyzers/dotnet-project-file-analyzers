using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("FullText}")]
public sealed record KeyValuePairSyntax : IniSyntax
{
    public KeySyntax? Key => Children.OfType<KeySyntax>().FirstOrDefault();

    public ValueSyntax? Value => Children.OfType<ValueSyntax>().FirstOrDefault();

    public KeyValuePair<string, string>? Kvp
        => Key is { } key && Value is { } val
        ? new(key.Text, val.Text)
        : null;

    public override IEnumerable<Diagnostic> GetDiagnostics()
    {
        var key = 0;
        var sig = 0;
        var val = 0;

        foreach (var token in Tokens)
        {
            switch (token.Kind)
            {
                case TokenKind.ValueToken:
                    if (++val > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "= or : is expected.")];
                    }
                    break;

                //case TokenKind.HeaderTextToken: sig++; break;

                //case TokenKind.HeaderStartToken:
                //    if (++key > 1)
                //    {
                //        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "[ is unexpected.")];
                //    }
                //    break;

                //case TokenKind.HeaderEndToken:
                //    if (++val > 1)
                //    {
                //        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "] is unexpected.")];
                //    }
                //    if (sig == 0)
                //    {
                //        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "] is expected.")];
                //    }
                //    break;
            }
        }
        return val == 0
            ? [Diagnostic.Create(Rule.Ini.InvalidHeader, SyntaxTree.GetLocation(Tokens[^1].LinePositionSpan), "] is expected.")]
            : [];
    }

    internal static IniSyntax New(Parser parser)
    {
        var root = IniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.WithLast(kvp => kvp with
                {
                    Span = kvp.Span + parser.Span,
                }),
                Span = s.Span + parser.Span,
            }),
        };
    }
}
