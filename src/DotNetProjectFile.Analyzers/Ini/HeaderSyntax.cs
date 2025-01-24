using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class HeaderSyntax(
    IniParser.SectionHeaderContext context,
    AbstractSyntaxTree tree) : IniSyntax(context, tree)
{
}
