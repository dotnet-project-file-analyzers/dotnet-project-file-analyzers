namespace Rules.MS_Build.Update_TargetFrameworks_with_TargetFrameworks;

public class Reports
{
    [Test]
    [Ignore("Buildalizer does not output any artifacts, hence nothing is analyzed.")]
    public void TFM_overriding_TFMs()
        => new UpdateTargetFrameworksWithTargetFrameworks()
        .ForProject("TfmsMixed.cs")
        .HasIssue(new Issue("Proj0027", "This <TargetFramework> will be ignored due to the earlier use of <TargetFrameworks>.")
        .WithSpan(06, 04, 06, 46));
}

public class Guards
{
    [Test]
    public void projects_with_TFM_only()
        => new UpdateTargetFrameworksWithTargetFrameworks()
        .ForProject("TargetFramework.cs")
        .HasNoIssues();

    [Test]
    public void projects_with_TFMs_only()
        => new UpdateTargetFrameworksWithTargetFrameworks()
        .ForProject("TargetFrameworksMultiple.cs")
        .HasNoIssues();


    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new UpdateTargetFrameworksWithTargetFrameworks()
        .ForProject(project)
        .HasNoIssues();
}
