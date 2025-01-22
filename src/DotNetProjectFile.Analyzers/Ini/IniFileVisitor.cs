
namespace DotNetProjectFile.Ini;

public sealed class IniFileVisitor : IniBaseVisitor<IniSyntax>
{
    public override IniSyntax VisitSection([NotNull] IniParser.SectionContext context)
    {

        var kvps = new List<KeyValuePairSyntax>();

        foreach (var child in context.children)
        {
            var x = Visit(child);

            if(x is KeyValuePairSyntax kvp)
            {
                kvps.Add(kvp);
            }

        }

        return new SectionSyntax(context);
    }

    public override IniSyntax VisitKeyValuePair([NotNull] IniParser.KeyValuePairContext context)
	{
        var key = new KeySyntax(context.key());
        var value = new ValueSyntax(context.value());

        return new KeyValuePairSyntax(key, value, context);
    }
}

