namespace Rules.Use_dotnet_project_file_analyzers;

public class Reports
{
    [Test]
    public void missing_analyzers()
        => new UseAnalyzers()
        .ForProject("EmptyProject.cs")
        .HasIssue(
            new Issue("Proj1000", "Use the .NET project file analyzers.").WithSpan(0, 1, 0, 2));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void Projects_with_analyzers(string project)
         => new UseAnalyzers()
        .ForProject(project)
        .HasNoIssues();
}
