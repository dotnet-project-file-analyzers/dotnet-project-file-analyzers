namespace DotNetProjectFile.Syntax;

internal sealed record RootSyntax(SyntaxTree? tree = null) : SyntaxNode
{
    public override SyntaxTree SyntaxTree { get; } = tree!;
}
