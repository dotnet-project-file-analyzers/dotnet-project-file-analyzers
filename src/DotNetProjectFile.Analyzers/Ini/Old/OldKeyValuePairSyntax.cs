using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{FullText}")]
public sealed record OldKeyValuePairSyntax : OldIniSyntax
{
    public OldKeySyntax? Key => Children.OfType<OldKeySyntax>().FirstOrDefault();

    public OldValueSyntax? Value => Children.OfType<OldValueSyntax>().FirstOrDefault();

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
                case TokenKind.KeyToken:
                    if (++key > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "= or : is expected.")];
                    }
                    break;

                case TokenKind.EqualsToken:
                case TokenKind.ColonToken:
                    if (key == 0 || ++sig > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), $"{token.Text} is unexpected.")];
                    }
                    break;

                case TokenKind.ValueToken:
                    if (sig == 0 || ++val > 1)
                    {
                        return [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(token.LinePositionSpan), "= or : is expected.")];
                    }
                    break;
            }
        }
        return val == 0
            ? [Diagnostic.Create(Rule.Ini.InvalidKeyValuePair, SyntaxTree.GetLocation(Tokens[^1].LinePositionSpan), "Value is missing.")]
            : [];
    }

    internal static OldIniSyntax New(Parser parser)
    {
        var root = OldIniFileSyntax.Root(parser);

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

    internal static OldIniSyntax Invalid(Parser parser)
    {
        var root = OldIniFileSyntax.Root(parser);

        return root with
        {
            Children = root.Sections.WithLast(s => s with
            {
                Children = s.KeyValuePairs.Add(new OldKeyValuePairSyntax()
                {
                    Span = parser.Span,
                }),
            }),
        };
    }
}
