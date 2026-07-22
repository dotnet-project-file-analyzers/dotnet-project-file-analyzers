using DotNetProjectFile.Analyzers.EditorConfig;

namespace Rules.EditorConfig.Use_equals_for_assignment;

public class Reports
{
    [Test]
    public void colon_signs() => new UseEqualsAssign()
        .ForInlineEditorconfig("""
        root = true

        [*]
        end_of_line: crlf
        
        """)
        .HasIssue(
            Issue.WRN("Proj4051", "Use '=' instead").WithSpan(03, 11, 03, 12));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new UseEqualsAssign()
        .ForProject(project)
        .HasNoIssues();
}

