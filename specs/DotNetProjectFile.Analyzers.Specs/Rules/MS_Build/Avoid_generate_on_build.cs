namespace Rules.MS_Build.Avoid_generate_on_build;

public class Reports
{
    [Test]
    public void on_generate_on_build_not_packable()
       => new AvoidGeneratePackageOnBuildWhenNotPackable()
       .ForProject("GeneratePackageNotPackable.cs")
       .HasIssue(
           new Issue("Proj0600", "Avoid defining <GeneratePackageOnBuild> node explicitly when <IsPackable> is 'false'.").WithSpan(6, 5, 6, 57));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("EmptyProject.cs")]
    public void Projects_without_issues(string project)
         => new AvoidGeneratePackageOnBuildWhenNotPackable()
        .ForProject(project)
        .HasNoIssues();
}
