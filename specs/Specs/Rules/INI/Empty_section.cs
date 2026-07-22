using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.Empty_section;

public class Reports
{
    [Test]
    public void empty_sections() => new EmptySection()
        .ForInlineEditorconfig("""
        root = true

        [*]
        end_of_line = crlf

        # Code files
        [*.{cs,json,cshtml,ts}]
        indent_style = space

        [empty.*]
        # Roslyn Analyzers
        [*.cs]
        vsspell_section_id = main
        """)
        .HasIssue(Issue.WRN("Proj4010", "Section [empty.*] is empty").WithSpan(09, 00, 10, 00));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new EmptySection()
        .ForProject(project)
        .HasNoIssues();
}

