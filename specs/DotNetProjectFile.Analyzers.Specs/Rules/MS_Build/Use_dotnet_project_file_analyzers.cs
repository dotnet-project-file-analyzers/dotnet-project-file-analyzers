namespace Rules.MS_Build.Use_dotnet_project_file_analyzers;

public class Reports
{
    [Test]
    public void missing_analyzers()
        => new UseAnalyzers()
        .ForProject("EmptyProject.cs")
        .HasIssue(
            Issue.WRN("Proj1000", "Use the .NET project file analyzers."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new UseAnalyzers()
        .ForProject(project)
        .HasNoIssues();
}
