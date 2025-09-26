namespace Rules.Use_forward_slashes_in_paths;

public class Reports
{
    [Test]
    public void backward_slashes() => new UseForwardSlashesInPaths()
       .ForProject("BackwardSlashes.cs")
       .HasIssues(
           Issue.WRN("Proj0023", "<Import Project> contains backward slashes" /*....*/).WithSpan(02, 2, 02, 44),
           Issue.WRN("Proj0023", "<DockerfileContext> contains backward slashes" /*.*/).WithSpan(06, 4, 06, 48),
           Issue.WRN("Proj0023", "<Compile Include> contains backward slashes" /*...*/).WithSpan(10, 4, 10, 66),
           Issue.WRN("Proj0023", "<Compile Link> contains backward slashes" /*......*/).WithSpan(10, 4, 10, 66),
           Issue.WRN("Proj0023", "<Folder Include> contains backward slashes" /*....*/).WithSpan(14, 4, 14, 41),
           Issue.WRN("Proj0023", "<None Remove> contains backward slashes" /*.......*/).WithSpan(18, 4, 18, 29));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new UseForwardSlashesInPaths()
        .ForProject(project)
        .HasNoIssues();
}
