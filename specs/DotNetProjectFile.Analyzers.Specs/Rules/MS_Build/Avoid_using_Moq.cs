namespace Rules.MS_Build.Avoid_using_Moq;

public class Reports
{
    [Test]
    public void on_Moq_dependency()
       => new AvoidUsingMoq()
       .ForProject("UsesMoq.cs")
       .HasIssue(new Issue("Proj1100", "Do not use Moq.").WithSpan(9, 04, 9, 55));
}

public class Guards
{
    [TestCase("PackagesWithAnalyzers.cs")]
    [TestCase("PackagesWithoutAnalyzers.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new AvoidUsingMoq()
        .ForProject(project)
        .HasNoIssues();
}
