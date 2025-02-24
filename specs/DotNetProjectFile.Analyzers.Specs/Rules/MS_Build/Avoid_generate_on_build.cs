namespace Rules.MS_Build.Avoid_generate_on_build;

public class Reports
{
    [Test]
    public void on_generate_on_build_not_packable()
       => new AvoidGeneratePackageOnBuildWhenNotPackable()
       .ForProject("GeneratePackageNotPackable.cs")
       .HasIssue(
           Issue.WRN("Proj0600", "Avoid defining <GeneratePackageOnBuild> node explicitly when <IsPackable> is 'false'").WithSpan(5, 04, 5, 57));
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
