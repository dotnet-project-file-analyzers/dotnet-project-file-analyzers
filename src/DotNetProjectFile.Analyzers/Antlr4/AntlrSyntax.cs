using Antlr4.Runtime;

namespace Antlr4;

[DebuggerDisplay("Name = {Name}, Text = {Text}")]
public class AntlrSyntax(ParserRuleContext context, AbstractSyntaxTree tree)
{
    /// <summary>The underlying abstract syntax tree.</summary>
    public AbstractSyntaxTree SyntaxTree { get; } = tree;

    protected ParserRuleContext Context { get; } = context;

    /// <inheritdoc cref="RuleContext.GetText()"/>>
    public string Text => Context.GetText();

    /// <summary>Gets the stream tokens.</summary>
    public IReadOnlyList<StreamToken> Tokens => SyntaxTree.Tokens(Context.SourceInterval);

    private string Name => GetType().Name;
}
