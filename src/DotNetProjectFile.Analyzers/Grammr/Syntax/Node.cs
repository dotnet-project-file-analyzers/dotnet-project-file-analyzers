using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr.Syntax;

public abstract class Node
{
    /// <summary>Gets the source span of the node.</summary>
    public abstract SourceSpan SourceSpan { get; }

    /// <summary>Gets the text span of the node.</summary>
    public TextSpan TextSpan => SourceSpan.Span;

    /// <summary>Gets the children of the node.</summary>
    public abstract IReadOnlyList<Node> Children { get; }

    /// <summary>Gets the (child) tokens.</summary>
    public virtual IReadOnlyCollection<SourceSpanToken> Tokens => [.. Children.SelectMany(c => c.Tokens)];

    /// <summary>Unwraps the children.</summary>
    public abstract IReadOnlyList<Node> Unwrap();

    [Pure]
    public static Node Token(SourceSpanToken token) => new TokenNode(token);

    [Pure]
    public static Node New(IReadOnlyList<Node> children) => new TokensNode(children);

    [Pure]
    public static Node New(Node node) => new TokensNode(node.Unwrap());

    [DebuggerDisplay("{Kind}: {SourceSpan.Text}")]
    private sealed class TokenNode(SourceSpanToken token) : Node
    {
        public string? Kind { get; } = token.Kind;

        /// <inheritdoc />
        public override SourceSpan SourceSpan { get; } = token.SourceSpan;

        /// <inheritdoc />
        public override IReadOnlyList<Node> Children => [];

        /// <inheritdoc />
        public override IReadOnlyCollection<SourceSpanToken> Tokens { get; } = [token];

        /// <inheritdoc />
        public override IReadOnlyList<Node> Unwrap() => [this];
    }

    [DebuggerDisplay("{Name}: {TextSpan}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    private sealed class TokensNode(IReadOnlyList<Node> children) : Node
    {
        private string Name => GetType().Name;

        /// <inheritdoc />
        public override IReadOnlyList<Node> Children { get; } = children;

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
        public override IReadOnlyList<Node> Unwrap() => Children;
    }
}
