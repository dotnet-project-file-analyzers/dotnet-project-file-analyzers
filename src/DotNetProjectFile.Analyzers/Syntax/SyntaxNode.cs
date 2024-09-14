using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Syntax;

public abstract record SyntaxNode
{
    public SyntaxNode? Parent { get; private set; }

    private readonly List<SyntaxNode> children = [];

    public IReadOnlyList<SyntaxNode> ChildNodes() => children;

    public ImmutableArray<SourceSpanToken> LeadingTrivia { get; init; } = [];

    public ImmutableArray<SourceSpanToken> Tokens { get; init; } = [];

    public ImmutableArray<SourceSpanToken> DescendantTrivia { get; init; } = [];

    public SourceText SourceText => Tokens[0].SourceSpan.SourceText;

    public override string ToString() => ToFullString();

    public string ToFullString()
    {
        var start = Tokens[0].Span.Start;
        var end = Tokens[^1].Span.End;
        return SourceText.ToString(new(start, end));
    }

    internal void SetParent(SyntaxNode parent)
    {
        Parent = parent;
        parent.children.Add(this);
    }
}
