namespace Rules.MS_Build.Test_project_should_not_be_packable;

public class Reports
{
    [Test]
    public void packable_test_project()
        => new TestProjectShouldNotBePackable()
        .ForProject("PackableTestProject.cs")
        .HasIssue(new Issue("Proj0450", "Set <IsPackable> to false.")
        .WithSpan(05, 04, 05, 33));

    [Test]
    public void implicit_packable_test_project()
        => new TestProjectShouldNotBePackable()
        .ForProject("ImplicitPackableTestProject.cs")
        .HasIssue(new Issue("Proj0450", "Set <IsPackable> to false.")
        .WithSpan(00, 00, 15, 10));
}

public class Guards
{
    [Test]
    public void non_packable_test_project()
        => new TestProjectShouldNotBePackable()
        .ForProject("TestProject.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void non_test_project(string project)
         => new TestProjectShouldNotBePackable()
        .ForProject(project)
        .HasNoIssues();
}
