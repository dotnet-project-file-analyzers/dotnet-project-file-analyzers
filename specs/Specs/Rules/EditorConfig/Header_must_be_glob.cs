using DotNetProjectFile.Analyzers.EditorConfig;

namespace Rules.EditorConfig.Header_must_be_glob;

public class Reports
{
    [Test]
    public void invalid_GLOBs() => new HeaderMustBeGlob()
        .ForInlineEditorconfig("""
        root = true
        [*]
        end_of_line = crlf
        insert_final_newline = true
        charset = utf-8

        # Code files
        [*.{cs,json}}]
        indent_style = space
        indent_size = 4

        # colon assigns
        [*.cs]
        indent_style: space
        indent_size: 4
        
        """)
        .HasIssue(Issue.ERR("Proj4050", "Header [*.{cs,json}}] is not a valid GLOB").WithSpan(07, 01, 07, 13));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new HeaderMustBeGlob()
        .ForProject(project)
        .HasNoIssues();
}

