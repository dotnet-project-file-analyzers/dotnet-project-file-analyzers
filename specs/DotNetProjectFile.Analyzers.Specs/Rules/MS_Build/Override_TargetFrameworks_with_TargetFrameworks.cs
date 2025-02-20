namespace Rules.MS_Build.Override_TargetFrameworks_with_TargetFrameworks;

public class Reports
{
    [Test]
    [Ignore("Buildalizer does not output any artifacts, hence nothing is analyzed.")]
    public void TFM_overriding_TFMs() => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject("TfmsMixed.cs")
        .HasIssue(Issue.WRN("Proj0027", "This <TargetFramework> will be ignored due to the earlier use of <TargetFrameworks>.")
        .WithSpan(06, 04, 06, 46));
}

public class Guards
{
    [Test]
    public void projects_with_TFM_only() => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject("TargetFramework.cs")
        .HasNoIssues();

    [Test]
    public void Single_TFM_and_conditional_TFMs() => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject("TargetFrameworkOverridesConditional.cs")
        .HasNoIssues();

    [Test]
    public void projects_with_TFMs_only() => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject("TargetFrameworksMultiple.cs")
        .HasNoIssues();


    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OverrideTargetFrameworksWithTargetFrameworks()
        .ForProject(project)
        .HasNoIssues();
}
