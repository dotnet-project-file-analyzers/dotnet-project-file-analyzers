using DotNetProjectFile.Ini;
using System.IO;
using SyntaxTree = DotNetProjectFile.Syntax.SyntaxTree;

namespace Parsing.INI_syntax;

public class Parses
{
    [Test]
    public void dot_editorconfig()
    {
        using var file = new FileStream("../../../../../.editorconfig", FileMode.Open, FileAccess.Read);
        var tree = SyntaxTree.Load(file);
        var syntax = IniFileSyntax.Parse(tree);

        syntax.Should().BeOfType<IniFileSyntax>();

        syntax.Tokens.Should().NotContain(t => t.Kind == TokenKind.UnparsableToken);
    }

    [Test]
    public void with_garbage()
    {
        var syntax = Parse.Syntax(@"global = false
some_key = value
invalid line
indenting = \t
[]"
        );

        var kvps = syntax.Sections.First().Kvps.ToArray();

        kvps.Should().BeEquivalentTo(new Dictionary<string, string>()
        {
            ["global"] = "false",
            ["some_key"] = "value",
            ["indenting"] = "\\t",
        });
    }
}


public class Parses_with_errors
{
    public class Header
    {
        [Test]
        public void no_text()
        {
            var syntax = Parse.Syntax("[]");

            syntax.Sections[0].Header!.GetDiagnostics().Should().HaveIssue(
                Issue.ERR("Proj4001", "] is expected.").WithSpan(0, 1, 0, 2));
        }

        [Test]
        public void multi_open()
        {
            var syntax = Parse.Syntax("[[header]");

            syntax.Sections[0].Header!.GetDiagnostics().Should().HaveIssue(
                Issue.ERR("Proj4001", "[ is unexpected.").WithSpan(0, 1, 0, 2));
        }

        [Test]
        public void no_closing()
        {
            var syntax = Parse.Syntax("[header\n");

            syntax.Sections[0].Header!.GetDiagnostics().Should().HaveIssue(
                Issue.ERR("Proj4001", "] is expected.").WithSpan(0, 1, 0, 7));
        }

        [Test]
        public void multi_closing()
        {
            var syntax = Parse.Syntax("[header]]");

            syntax.Sections[0].Header!.GetDiagnostics().Should().HaveIssue(
                Issue.ERR("Proj4001", "] is unexpected.").WithSpan(0, 8, 0, 9));
        }
    }

    public class Key_value_pair
    {
        [Test]
        public void no_text()
        {
            var syntax = Parse.Syntax(@"key1 == value");

            syntax.Sections[0].KeyValuePairs[0].GetDiagnostics().Should().HaveIssue(
                Issue.ERR("Proj4001", "] is expected.").WithSpan(0, 1, 0, 2));
        }
    }
}

file static class Parse
{
    public static IniFileSyntax Syntax(string text)
    {
        var tree = SyntaxTree.Parse(text);
        var synstax = IniFileSyntax.Parse(tree);
        synstax.Should().NotBeNull();
        return synstax!;
    }
}
