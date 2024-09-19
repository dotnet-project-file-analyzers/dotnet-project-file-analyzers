using DotNetProjectFile.Ini;
using System.IO;

namespace Parsing.INI_syntax;

public class Parses
{
    [Test]
    public void dot_editorconfig()
    {
        using var file = new FileStream("../../../../../.editorconfig", FileMode.Open, FileAccess.Read);
        var tree = DotNetProjectFile.Syntax.SyntaxTree.From(file);
        var syntax = IniFileSyntax.Parse(tree);

        syntax.Should().BeOfType<IniFileSyntax>();
    }

    [Test]
    public void with_garbage()
    {
        var tree = DotNetProjectFile.Syntax.SyntaxTree.Parse(@"global = false
some_key = value
invalid line
indenting = \t"
        );

        var syntax = IniFileSyntax.Parse(tree);
        syntax.Should().BeOfType<IniFileSyntax>();

        var kvps = syntax.Sections.Single().Kvps.ToArray();

        kvps.Should().BeEquivalentTo(new Dictionary<string, string>()
        {
            ["global"] = "false",
            ["some_key"] = "value",
            ["indenting"] = "\\t",
        });
    }
}
