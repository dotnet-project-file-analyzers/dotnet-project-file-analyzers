using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class SectionSyntax(
    //IReadOnlyList<KeyValuePairSyntax> pairs,
    IniParser.FileContext context,
    AbstractSyntaxTree tree) : IniSyntax(context, tree)
{
    //public IReadOnlyList<KeyValuePairSyntax> Pairs { get; } = pairs;
}
