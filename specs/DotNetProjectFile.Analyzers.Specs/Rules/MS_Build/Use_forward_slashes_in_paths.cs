namespace Use_forward_slashes_in_paths;

public class Reports
{
    [Test]
    public void on_not_ordered_by_type()
       => new UseForwardSlashesInPaths()
       .ForProject("BackwardSlashes.cs")
       .HasIssues(
           new Issue("Proj0023", "<Import Project> contains backward slashes.").WithSpan(2, 3, 2, 44),
           new Issue("Proj0023", "<Compile Include> contains backward slashes.").WithSpan(9, 5, 9, 66),
           new Issue("Proj0023", "<Compile Link> contains backward slashes.").WithSpan(9, 5, 9, 66),
           new Issue("Proj0023", "<Folder Include> contains backward slashes.").WithSpan(13, 5, 13, 41),
           new Issue("Proj0023", "<None Remove> contains backward slashes.").WithSpan(17, 5, 17, 29));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new UseForwardSlashesInPaths()
        .ForProject(project)
        .HasNoIssues();
}
