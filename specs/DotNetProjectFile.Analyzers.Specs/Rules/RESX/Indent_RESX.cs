namespace Rules.RESX.Indent_RESX;

public class Reports
{
    [Test]
    public void faulty_indented()
        => new Resx.IndentResx()
        .ForProject("FaultyIndenting.cs")
        .HasIssues(
            new Issue("Proj2100", "The element <resheader> has not been properly indented." /*.*/).WithSpan(05, 03, 05, 27),
            new Issue("Proj2100", "The element <data> has not been properly indented." /*......*/).WithSpan(11, 03, 11, 41),
            new Issue("Proj2100", "The element </data> has not been properly indented." /*.....*/).WithSpan(13, 02, 13, 09),
            new Issue("Proj2100", "The element <value> has not been properly indented." /*.....*/).WithSpan(15, 07, 15, 13));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new Resx.IndentResx()
        .ForProject(project)
        .HasNoIssues();
}
