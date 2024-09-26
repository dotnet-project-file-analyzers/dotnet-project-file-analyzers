namespace Rules.MS_Build.Generate_on_build_conditionally;

public class Reports
{
    [Test]
    public void on_unconditional_package_generation()
       => new GeneratePackageOnBuildConditionally()
       .ForProject("GeneratePackageOnBuildUnconditionally.cs")
       .HasIssue(
           new Issue("Proj0242", "Add a condition to <GeneratePackageOnBuild>.").WithSpan(4, 4, 4, 57));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("EmptyProject.cs")]
    public void Projects_without_issues(string project)
         => new GeneratePackageOnBuildConditionally()
        .ForProject(project)
        .HasNoIssues();
}
