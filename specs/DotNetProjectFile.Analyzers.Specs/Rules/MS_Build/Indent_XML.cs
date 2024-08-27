namespace Rules.MS_Build.Indent_XML;

public class Reports
{
    [Test]
    public void malicious_indented()
        => new IndentXml()
        .ForProject("MaliciousIndenting.cs")
        .HasIssues(
            new Issue("Proj1700", "The element <PropertyGroup> has not been properly indented." /*...*/).WithSpan(4, 04, 4, 18),
            new Issue("Proj1700", "The element <TargetFramework> has not been properly indented." /*.*/).WithSpan(5, 02, 5, 18),
            new Issue("Proj1700", "The element </PropertyGroup> has not been properly indented." /*..*/).WithSpan(6, 05, 6, 21),
            new Issue("Proj1700", "The element <Compile> has not been properly indented." /*.........*/).WithSpan(9, 04, 9, 43));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new IndentXml()
        .ForProject(project)
        .HasNoIssues();
}
