using DotNetProjectFile.Syntax;

namespace DotNetProjectFile.Parsing.Internal;

internal sealed class AddSyntaxNode(Grammar grammar, CreateSyntaxNode create) : Grammar
{
    private readonly Grammar Inner = grammar;
    private readonly CreateSyntaxNode Create = create;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => Inner.Match(parser) is { State: not Matching.NoMatch } match
        ? match.WithSyntax(Create(match))
        : parser.NoMatch();
}
