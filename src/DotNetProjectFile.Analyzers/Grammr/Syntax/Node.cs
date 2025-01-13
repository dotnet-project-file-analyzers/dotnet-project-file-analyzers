using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Syntax;

[Inheritable]
[DebuggerDisplay("{Name}: {TextSpan}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public class Node(IReadOnlyList<TreeNode> children) : TreeNode
{
    public Node(TreeNode node) : this(node.Unwrap()) { }

    private string Name => GetType().Name;

    /// <inheritdoc />
    public override IReadOnlyList<TreeNode> Children { get; } = children;

    /// <summary>Gets the source span of the node.</summary>
    public override SourceSpan SourceSpan
    {
        get
        {
            if (Children.Count == 0) return default;

            var start = Children[0].SourceSpan.Start;
            var end = Children[^1].SourceSpan.End;

            return new(Children[0].SourceSpan.Source, new TextSpan(start, end - start));
        }
    }

    /// <inheritdoc />
    public override IReadOnlyList<TreeNode> Unwrap() => Children;
}
