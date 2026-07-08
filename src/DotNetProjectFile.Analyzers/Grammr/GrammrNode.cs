using DotNetProjectFile.Collections;
using Microsoft.CodeAnalysis.Text;

namespace Grammr;

[DebuggerDisplay("{FullSpan.ToString()}")]
public abstract class GrammrNode(SliceSpan span, GrammrTree tree)
{
    /// <summary>Gets the parent of the node.</summary>
    public GrammrNode? Parent { get; internal set; }

    /// <summary>Gets the source tree of the node.</summary>
    public GrammrTree SourceTree { get; } = tree;

    /// <summary>Gets the children of the node.</summary>
    public GrammrChildren Children { get; } = [];

    /// <summary>Gets the token span.</summary>
    public SliceSpan TokenSpan { get; } = span;

    /// <summary>Gets the text span of the node.</summary>
    public TextSpan TextSpan
    {
        get
        {
            var f = Tokens[0];
            var l = Tokens[^1];
            return new(f.Start, l.End - f.Start);
        }
    }

    /// <summary>Gets the line position span of the node.</summary>
    public LinePositionSpan LinePositionSpan => SourceTree.SourceText.Lines.GetLinePositionSpan(TextSpan);

    /// <summary>Gets the full span of the node.</summary>
    public ReadOnlySpan<char> FullSpan
    {
        get
        {
            var f = Tokens[0];
            var l = Tokens[^1];
            return Stream.Span[f.Start..l.End];
        }
    }

    /// <summary>Gets the tokens of the node.</summary>
    public Slice<Token> Tokens => Stream[TokenSpan];

    /// <summary>Gets the underlying token stream.</summary>
    public TokenStream Stream => SourceTree.Stream;

    /// <summary>Adds a child to the node..</summary>
    /// <remarks>
    /// By doing so, the parent is set to the added child.
    /// </remarks>
    public void AddChild(GrammrNode? node)
    {
        SourceTree.ThrowIfFinal();

        if (node is not null)
        {
            Children.Add(node);
            node.Parent = this;
        }
    }

    /// <summary>Adds children to the node..</summary>
    /// <remarks>
    /// By doing so, the parent is set to the added children.
    /// </remarks>
    public void AddChildren(IEnumerable<GrammrNode?> nodes)
    {
        SourceTree.ThrowIfFinal();

        foreach (var node in nodes.OfType<GrammrNode>())
        {
            Children.Add(node);
            node.Parent = this;
        }
    }

    /// <summary>Gets diagnostics.</summary>
    [Pure]
    public virtual IEnumerable<Diagnostic> GetDiagnostics()
        => Children.SelectMany(c => c.GetDiagnostics());
}
