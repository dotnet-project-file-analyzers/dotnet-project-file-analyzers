using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.Define_keys_once;

public class Reports
{
    [Test]
    public void empty_sections() => new DefineKeysOnce().ForInlineEditorconfig("""
        root = true

        [*]
        end_of_line = crlf

        # Code files
        [*.cs}]
        end_of_line = crlf
        indent_style = space
        indent_style = tab
        
        """)
        .HasIssue(Issue.WRN("Proj4011", "'indent_style' has already been defined at line 9").WithSpan(09, 00, 09, 13));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EmptySection()
        .ForProject(project)
        .HasNoIssues();
}

