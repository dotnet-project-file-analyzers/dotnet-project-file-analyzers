namespace Rules.MS_Build.Define_is_publishable;

public class Reports
{
    [Test]
    public void on_no_is_publishable()
       => new DefineIsPublishable()
       .ForProject("EmptyProject.cs")
       .HasIssue(Issue.WRN("Proj0400", "Define the <IsPublishable> node explicitly"));
}

public class Guards
{
    [Test]
    public void test_project()
        => new DefineIsPublishable()
        .ForProject("ImplicitPackablePublishableTestProject.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineIsPublishable()
        .ForProject(project)
        .HasNoIssues();
}
