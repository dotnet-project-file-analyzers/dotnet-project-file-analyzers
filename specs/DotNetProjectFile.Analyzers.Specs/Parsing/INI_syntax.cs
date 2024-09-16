using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;
using System.IO;

namespace Parsing.INI_syntax;

public class Parses
{
    [Test]
    public void dot_editorconfig()
    {
        using var file = new FileStream("../../../../../.editorconfig", FileMode.Open, FileAccess.Read);
        var source = SourceText.From(file);
        
        var syntax = IniFileSyntax.Parse(source);

        syntax.Should().BeOfType<IniFileSyntax>();
    }
}
