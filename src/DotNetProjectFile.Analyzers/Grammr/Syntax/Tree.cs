using Grammr.Text;

namespace Grammr.Syntax;

public sealed class Tree
{
    public Tree(Node root, TokenStream tokens, string? errorMessage)
    {
        Root = root;
        Tokens = tokens;
        ErrorMessage = errorMessage;
    }

    /// <summary>Gets the root node of the syntax tree.</summary>
    public Node Root { get; }

    /// <summary>The token stream of the tree.</summary>
    public TokenStream Tokens { get; }

    /// <summary>Indicates if the parsing was successful.</summary>
    public bool Success => ErrorMessage is null && Tokens.Remaining.Length == 0;

    /// <summary>The (optional) error message.</summary>
    public string? ErrorMessage { get; }
}
