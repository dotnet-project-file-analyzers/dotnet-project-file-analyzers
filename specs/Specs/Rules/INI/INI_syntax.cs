using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.INI_syntax;

public class Reports
{
    [Test]
    public void errors() => new IniSyntaxAnalyzer()
        .ForInlineEditorconfig("""
        root = true

        [No Header
        KeyWithoutAssignement Value
        KeyWithoutValue =
        Key1 = Value

        # Section
        [*]
        Key2 = value # With comment
        KeyWith Comment = #Comment

        []
        key = value

        [[Header]
        
        """)
        .HasIssues(
            Issue.ERR("Proj4001", "']' is expected" /*........*/).WithSpan(02, 10, 03, 00),
            Issue.ERR("Proj4002", "'=' or ':' is expected" /*.*/).WithSpan(03, 21, 03, 22),
            Issue.ERR("Proj4002", "Value is expected" /*......*/).WithSpan(04, 17, 05, 00),
            Issue.ERR("Proj4002", "'=' or ':' is expected" /*.*/).WithSpan(10, 07, 10, 08),
            Issue.ERR("Proj4000", "'=' is unexpected" /*......*/).WithSpan(10, 16, 10, 26),
            Issue.ERR("Proj4001", "header text is missing" /*.*/).WithSpan(12, 01, 12, 02));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new IniSyntaxAnalyzer()
        .ForProject(project)
        .HasNoIssues();
}

