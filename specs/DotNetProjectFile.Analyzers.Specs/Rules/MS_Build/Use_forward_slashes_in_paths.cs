namespace Rules.Use_forward_slashes_in_paths;

public class Reports
{
    [Test]
    public void on_not_ordered_by_type()
       => new UseForwardSlashesInPaths()
       .ForProject("BackwardSlashes.cs")
       .HasIssues(
           Issue.WRN("Proj0023", "<Import Project> contains backward slashes." /*..*/).WithSpan(02, 2, 02, 44),
           Issue.WRN("Proj0023", "<Compile Include> contains backward slashes." /*.*/).WithSpan(09, 4, 09, 66),
           Issue.WRN("Proj0023", "<Compile Link> contains backward slashes." /*....*/).WithSpan(09, 4, 09, 66),
           Issue.WRN("Proj0023", "<Folder Include> contains backward slashes." /*..*/).WithSpan(13, 4, 13, 41),
           Issue.WRN("Proj0023", "<None Remove> contains backward slashes." /*.....*/).WithSpan(17, 4, 17, 29));
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
