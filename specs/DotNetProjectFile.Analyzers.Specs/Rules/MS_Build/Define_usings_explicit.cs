namespace Rules.MS_Build.Define_usings_explicit;

public class Reports
{
    [Test]
    public void implicit_usings()
        => new DefineUsingsExplicit()
        .ForProject("ImplicitUsings.cs")
        .HasIssue(
            new Issue("Proj0003", "Define usings explicit.").WithSpan(4, 4, 4, 43));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineUsingsExplicit()
        .ForProject(project)
        .HasNoIssues();
}
