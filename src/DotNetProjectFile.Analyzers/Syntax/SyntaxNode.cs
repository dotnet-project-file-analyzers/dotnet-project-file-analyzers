using DotNetProjectFile.Collections;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Syntax;

public abstract record SyntaxNode
{
    public virtual SyntaxTree SyntaxTree => tree ??= ResolveTree();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private SyntaxTree? tree;

    private SyntaxTree ResolveTree()
    {
        var parent = Parent;
        while (parent is { })
        {
            if (parent is RootSyntax t) return t.SyntaxTree;
            parent = parent.Parent;
        }
        throw new InvalidOperationException("SyntaxTree could not be resolved.");
    }

    public SyntaxNode? Parent { get; private set; }

    public ImmutableArray<SyntaxNode> Children { get; init; } = [];

    public IReadOnlyList<SourceSpanToken> Tokens => new Slice<SourceSpanToken>(Span, SyntaxTree.Tokens);

    public SliceSpan Span { get; init; }

    public LinePositionSpan LinePositionSpan => SyntaxTree.SourceText.Lines.GetLinePositionSpan(new(Span.Start, Span.End - Span.Start));

    public string FullText
    {
        get
        {
            var start = Tokens[0].Span.Start;
            var end = Tokens[^1].Span.End;
            return SyntaxTree.SourceText.ToString(new(start, end));
        }
    }

    public override string ToString() => FullText;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected virtual string DebuggerDisplay => $"Syntax = {GetType().Name.Replace("Syntax", string.Empty)}, Tokens = {Span.Size}";

    internal void SetParent(SyntaxNode parent)
    {
        Parent = parent;
        foreach (var child in Children)
        {
            child.SetParent(this);
        }
    }
}
