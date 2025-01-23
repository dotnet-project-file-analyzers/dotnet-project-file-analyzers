using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class IniFileVisitor(AbstractSyntaxTree tree) : IniBaseVisitor<IniSyntax>
{
    private readonly AbstractSyntaxTree Tree = tree;

    public override IniSyntax VisitFile([NotNull] IniParser.FileContext context)
    {
        var pairs = new List<KeyValuePairSyntax>();

        foreach (var child in context.children.Select(Visit))
        {
            if(child is KeyValuePairSyntax pair)
            {
                pairs.Add(pair);
            }    
        }

		return new IniFileSyntax([new SectionSyntax(pairs, context, Tree)], context, Tree);
	}

    public override IniSyntax VisitKeyValuePair([NotNull] IniParser.KeyValuePairContext context)
    {
        var key = new KeySyntax(context.key(), Tree);
        var value = new ValueSyntax(context.value(), Tree);

        return new KeyValuePairSyntax(key, value, context, Tree);
    }
}

