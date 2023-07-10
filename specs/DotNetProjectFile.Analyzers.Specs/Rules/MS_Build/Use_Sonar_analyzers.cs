namespace Rules.MS_Build.Use_Sonar_analyzers;

public class Reports
{
    [Test]
    public void missing_analyzer_for_CSharp()
        => new UseSonarAnalyzers()
        .ForProject("EmptyProject.cs")
        .HasIssue(
            new Issue("Proj1003", "Add SonarAnalyzer.CSharp."));

    [Test]
    public void missing_analyzer_for_Visual_Basic()
        => new UseSonarAnalyzers()
        .ForProject("EmptyProjectVB.vb")
        .HasIssue(
            new Issue("Proj1003", "Add SonarAnalyzer.VisualBasic."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void Projects_with_analyzers(string project)
         => new UseSonarAnalyzers()
        .ForProject(project)
        .HasNoIssues();
}
