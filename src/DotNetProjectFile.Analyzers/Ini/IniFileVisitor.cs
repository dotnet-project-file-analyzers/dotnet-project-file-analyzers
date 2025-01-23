using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class IniFileVisitor(AbstractSyntaxTree tree) : IniBaseVisitor<IniSyntax>
{
    private readonly AbstractSyntaxTree Tree = tree;

    public override IniSyntax VisitFile([NotNull] IniParser.FileContext context)
    {
        //var sections = context.children
        //    .Select(Visit)
        //    .OfType<SectionSyntax>()
        //    .ToArray();

		return new IniFileSyntax([], context, Tree);
	}

	//public override IniSyntax VisitSection([NotNull] IniParser.SectionContext context)
 //   {

 //       var pairs = new List<KeyValuePairSyntax>();

 //       foreach (var child in context.children)
 //       {
 //           var x = Visit(child);

 //           if(x is KeyValuePairSyntax kvp)
 //           {
 //               pairs.Add(kvp);
 //           }

 //       }

 //       return new SectionSyntax(pairs, context, Tree);
 //   }

 //   public override IniSyntax VisitKeyValuePair([NotNull] IniParser.KeyValuePairContext context)
	//{
 //       var key = new KeySyntax(context.key(), Tree);
 //       var value = new ValueSyntax(context.value(), Tree);

 //       return new KeyValuePairSyntax(key, value, context, Tree);
 //   }
}

