using DotNetProjectFile.Collections;
using Grammr.Text;
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
            if (parent is RootSyntax t)
            {
                return t.SyntaxTree;
            }
            parent = parent.Parent;
        }
        throw new InvalidOperationException("SyntaxTree could not be resolved.");
    }

    public SyntaxNode? Parent { get; private set; }

    public ImmutableArray<SyntaxNode> Children { get; init; } = [];

    public IReadOnlyList<SourceSpanToken> Tokens => new Slice<SourceSpanToken>(Span, SyntaxTree.Tokens);

    public SliceSpan Span { get; init; }

    public LinePositionSpan LinePositionSpan => SyntaxTree.SourceText.Lines.GetLinePositionSpan(new(Tokens[0].Span.Start, Tokens[^1].Span.End - Tokens[0].Span.Start));

    /// <summary>Gets the full text of the node.</summary>
    public string FullText => string.Concat(Tokens.Select(t => t.Text));

    /// <summary>Gets the diagnostics for the node.</summary>
    [Pure]
    public virtual IEnumerable<Diagnostic> GetDiagnostics() => [];

    /// <inheritdoc />
    [Pure]
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
