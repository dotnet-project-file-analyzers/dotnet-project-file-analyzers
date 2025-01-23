using Antlr4.Runtime;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Ini;

public sealed class IniFile
{
	public required IniFileSyntax Sytnax { get; init; }

	public static IniFile Parse(SourceText source)
	{
		var stream = new AntlrInputStream(source.ToString());
		var lexer = new IniLexer(stream);
		var tokens = new CommonTokenStream(lexer);
		var parser = new IniParser(tokens);
        
		var visitor = new IniFileVisitor(new(tokens, parser.Vocabulary));

		var syntax = (IniFileSyntax)visitor.Visit(parser.file());

		return new()
		{
			Sytnax = syntax 
		};
	}
}
