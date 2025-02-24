using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.Empty_section;

public class Reports
{
    [Test]
    public void empty_sections() => new EmptySection()
        .ForProject("IniFileWithEmptySection.cs")
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

