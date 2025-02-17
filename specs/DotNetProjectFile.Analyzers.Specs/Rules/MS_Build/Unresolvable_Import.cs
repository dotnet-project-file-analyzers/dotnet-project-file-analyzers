namespace Rules.MS_Build.Unresolvable_Import;

public class Reports
{
    [Test]
    public void empty_includes() => new UnresolvableImport()
    .ForProject("UnresolvableImport.cs")
        .HasIssue(Issue.WRN("Proj0034", "The <Import> '$(MSBuildThisFileDirectory)common.props' could not be resolved by the analyzer.").WithSpan(02, 02, 02, 62));

    [Test]
    public void on_rule_that_is_not_affected() => new TrackToDoTags()
    .ForProject("UnresolvableImport.cs")
        .HasIssue(Issue.WRN("Proj3001", "Complete the task associated to this \"TODO\" comment."));
}

public class Guards
{
    [Test]
    public void rule_that_is_affected() => new ConfigureCentralPackageVersionManagement()
    .ForProject("UnresolvableImport.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project) => new UnresolvableImport()
        .ForProject(project)
        .HasNoIssues();
}
