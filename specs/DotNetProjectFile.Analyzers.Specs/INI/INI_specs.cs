using Antlr4.Runtime;
using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.IO;

namespace Grammars.INI_specs;

public class Parses
{
    [Test]
    public void single_line()
    {
        using var file = new FileStream("C:/code/dotnet-project-file-analyzers-antlr/.editorconfig", FileMode.Open, FileAccess.Read);
        var stream = new AntlrInputStream(file);
        var lexer = new IniLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new IniParser(tokens);

        var visitor = new IniFileVisitor();

        var result = visitor.Visit(parser.file());
    }
}
