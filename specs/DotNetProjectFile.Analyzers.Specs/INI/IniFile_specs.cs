using DotNetProjectFile.Ini;

namespace Specs.INI.IniFile_specs;

public class Parses
{
    [TestCase("ini-0001-lines.ini", 1)]
    [TestCase("ini-0008-lines.ini", 5)]
    [TestCase("ini-0027-lines.ini", 14)]
    [TestCase("ini-0036-lines.ini", 11)]
    [TestCase("ini-1220-lines.ini", 1177)]
    public void Embedded(string file, int count)
    {
        var tree = Files.Tree(file);
        var ini = IniFile.Parse(tree);
        ini.Sections.SelectMany(s => s.Entries).Should().HaveCount(count);

        ini.GetDiagnostics().Should().HaveNoIssues();
    }

    [Test]
    public void Leading_whitespace()
    {
        var tree = Test.Tree("  root:true\n");
        var ini = IniFile.Parse(tree);
        ini.Sections.Single().Entries.Single().Should().BeEquivalentTo(new
        {
            Key = new { Text = "root" },
            Value = new { Text = "true" },
        });

        ini.GetDiagnostics().Should().HaveNoIssues();
    }

    [Test]
    public void Starting_with_comment()
    {
        var tree = Test.Tree("""
            # top-most EditorConfig file
            root = true
            """);

        var ini = IniFile.Parse(tree);
        ini.Sections.Single().Entries.Single().Should().BeEquivalentTo(new
        {
            Key = new { Text = "root" },
            Value = new { Text = "true" },
        });

        ini.GetDiagnostics().Should().HaveNoIssues();
    }
}
public class Parses_with_issues
{
    [Test]
    public void Header_without_closing_tag()
    {
        var tree = Test.Tree("[No Header\n");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4001", "']' is expected"));
    }

    [Test]
    public void Header_without_text()
    {
        var tree = Test.Tree("[]");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4001", "header text is missing"));
    }

    [Test]
    public void Header_followed_by_noise()
    {
        var tree = Test.Tree("[*.csproj] some noise");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4000", "'s' is unexpected"));
    }

    [Test]
    public void KVP_without_assign()
    {
        var tree = Test.Tree("root true");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4002", "'=' or ':' is expected"));
    }

    [Test]
    public void KVP_without_value()
    {
        var tree = Test.Tree("root = ");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4002", "Value is expected"));
    }

    [Test]
    public void KVP_with_noise()
    {
        var tree = Test.Tree("root = value some noise");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4000", "'s' is unexpected"));
    }


    [Test]
    public void None_value_start()
    {
        var tree = Test.Tree("@at = value");
        var ini = IniFile.Parse(tree);

        ini.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj4000", "'@' is unexpected"));
    }
}
