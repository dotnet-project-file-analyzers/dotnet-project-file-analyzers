using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;

namespace Grammars.INI_specs;

public class Parses
{
    private const string ASSIGN = nameof(ASSIGN);
    private const string KEY = "TEXT";
    private const string VALUE = "TEXT";
    private const string WS = nameof(WS);
    private const string CRLF = nameof(CRLF);

    [TestCase("=")]
    [TestCase(":")]
    public void key_value_pair(string assign)
    {
        var source = SourceText.From($"root{assign}true");
        var file = IniFile.Parse(source).Sytnax;

        file.Tokens.Should().BeEquivalentTo(
        [
            new{ Text = "root", Kind = KEY },
            new{ Text = assign, Kind = ASSIGN },
            new{ Text = "true", Kind = VALUE },
        ]);
    }

    [Test]
    public void key_value_pairs()
    {
        var source = SourceText.From($"root=true\nvalue=okay");
        var file = IniFile.Parse(source).Sytnax;

        file.Tokens.Should().BeEquivalentTo(
        [
            new{ Text = "root", Kind = KEY },
            new{ Text = "=", Kind = ASSIGN },
            new{ Text = "true", Kind = VALUE },
            new{ Text = "\n", Kind = CRLF },
            new{ Text = "value", Kind = KEY },
            new{ Text = "=", Kind = ASSIGN },
            new{ Text = "okay", Kind = VALUE },
        ]);
    }

    [Test]
    public void empty_lines()
    {
        var source = SourceText.From($"root=true\n\r\n     \nvalue=okay");
        var file = IniFile.Parse(source).Sytnax;

        file.Tokens.Should().BeEquivalentTo(
        [
            new{ Text = "root", Kind = KEY },
            new{ Text = "=", Kind = ASSIGN },
            new{ Text = "true", Kind = VALUE },
            new{ Text = "\n", Kind = CRLF },
            new{ Text = "\r\n", Kind = CRLF },
            new{ Text = "     ", Kind = WS },
            new{ Text = "\n", Kind = CRLF },
            new{ Text = "value", Kind = KEY },
            new{ Text = "=", Kind = ASSIGN },
            new{ Text = "okay", Kind = VALUE },
        ]);
    }

    [Test]
    public void key_value_pair_with_spaces()
    {
        var source = SourceText.From("\troot  = true\r\n");
        var file = IniFile.Parse(source).Sytnax;

        file.Tokens.Should().BeEquivalentTo(
        [
            new{ Text = "\t", Kind = WS },
            new{ Text = "root", Kind = KEY },
            new{ Text = "  ", Kind = WS },
            new{ Text = "=", Kind = ASSIGN },
            new{ Text = " ", Kind = WS },
            new{ Text = "true", Kind = VALUE },
            new{ Text = "\r\n", Kind = CRLF },
        ]);
    }
}
