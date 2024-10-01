using DotNetProjectFile.Parsing;

namespace DotNetProjectFile.Git;

[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record PatternSyntax : Syntax.SyntaxNode
{
    public Glob? Pattern { get; init; }

    /// <inheritdoc />
    public override string ToString() => Tokens[^1].Text;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected override string DebuggerDisplay => Pattern.HasValue
        ? ToString()
        : $"{this}, Unparsable";

    internal static GitIgnoreSyntax New(Parser parser)
    {
        var root = parser.Syntax as GitIgnoreSyntax ?? new();
        return root with
        {
            Children = root.Children.Add(new PatternSyntax()
            {
                Pattern = Glob.TryParse(parser.Tokens[^1].Text),
                Span = parser.Span,
            }),
        };
    }
}
