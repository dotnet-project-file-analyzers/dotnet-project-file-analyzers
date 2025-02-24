namespace Rules.MS_Build.NuGet_packages.Define_is_packable;

public class Reports
{
    [Test]
    public void on_no_is_packable()
       => new DefineIsPackable()
       .ForProject("NoIsPackable.cs")
       .HasIssue(
           Issue.WRN("Proj0200", "Define the <IsPackable> node explicitly"));
}

public class Guards
{
    [Test]
    public void test_project()
        => new DefineIsPackable()
        .ForProject("ImplicitPackablePublishableTestProject.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineIsPackable()
        .ForProject(project)
        .HasNoIssues();
}
