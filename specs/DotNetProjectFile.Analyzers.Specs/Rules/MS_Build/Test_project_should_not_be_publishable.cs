namespace Rules.MS_Build.Test_project_should_not_be_publishable;

public class Reports
{
    [Test]
    public void publishable_test_project()
        => new TestProjectShouldNotBePublishable()
        .ForProject("PackableTestProject.cs")
        .HasIssue(new Issue("Proj0451", "Set <IsPublishable> to false.")
        .WithSpan(06, 04, 06, 39));

    [Test]
    public void implicit_publishable_test_project()
        => new TestProjectShouldNotBePublishable()
        .ForProject("ImplicitPackableTestProject.cs")
        .HasIssue(new Issue("Proj0451", "Set <IsPublishable> to false.")
        .WithSpan(00, 00, 15, 10));
}

public class Guards
{
    [Test]
    public void non_publishable_test_project()
        => new TestProjectShouldNotBePublishable()
        .ForProject("TestProject.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void non_test_project(string project)
         => new TestProjectShouldNotBePublishable()
        .ForProject(project)
        .HasNoIssues();
}
