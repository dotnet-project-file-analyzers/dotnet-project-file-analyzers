namespace DotNetProjectFile.Ini;

public sealed class IniFileSyntax(
	IReadOnlyList<SectionSyntax> sections,
	IniParser.FileContext context) : IniSyntax(context)
{
	public IReadOnlyList<SectionSyntax> Sections { get; } = sections;
}
