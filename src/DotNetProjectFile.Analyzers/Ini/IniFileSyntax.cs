using Antlr4;

namespace DotNetProjectFile.Ini;

public sealed class IniFileSyntax(
	IReadOnlyList<SectionSyntax> sections,
	IniParser.FileContext context,
    AbstractSyntaxTree tree) : IniSyntax(context, tree)
{
	public IReadOnlyList<SectionSyntax> Sections { get; } = sections;
}
