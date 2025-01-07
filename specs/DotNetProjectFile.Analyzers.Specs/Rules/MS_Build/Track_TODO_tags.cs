namespace Rules.MS_Build.Track_TODO_tags;

public class Reports
{
    [Test]
    public void TODOs_and_FIXMEs() => new TrackToDoTags()
        .ForProject("ToDo.cs")
        .HasIssues(
            Issue.WRN("Proj0033", @"Complete the task associated to this ""Fixme"" comment.").WithSpan(08, 54, 10, 02),
            Issue.WRN("Proj0033", @"Complete the task associated to this ""FIX ME"" comment.").WithSpan(12, 06, 14, 02),
            Issue.WRN("Proj0033", @"Complete the task associated to this ""TODO"" comment.").WithSpan(06, 06, 06, 34),
            Issue.WRN("Proj0033", @"Complete the task associated to this ""to-do"" comment.").WithSpan(16, 56, 18, 02));
}

public class Has_no_issues
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void For(string project) => new TrackToDoTags()
        .ForProject(project)
        .HasNoIssues();
}
