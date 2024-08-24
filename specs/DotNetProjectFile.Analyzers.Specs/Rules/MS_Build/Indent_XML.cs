namespace Rules.MS_Build.Indent_XML;

public class Reports
{
    [Test]
    public void malicious_indented()
        => new IndentXml()
        .ForProject("MaliciousIndenting.cs")
        .HasIssue(
            new Issue("Proj0003", "Define usings explicit.").WithSpan(4, 5, 4, 43));
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
