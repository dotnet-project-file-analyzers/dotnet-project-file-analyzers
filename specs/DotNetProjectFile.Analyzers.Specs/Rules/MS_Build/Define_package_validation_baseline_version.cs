namespace Rules.MS_Build.Define_package_validation_baseline_version;

public class Reports
{
    [Test]
    public void on_missing_node()
       => new DefinePackageValidationBaselineVersion()
       .ForProject("NoBaselineVersion.cs")
       .HasIssue(
           new Issue("Proj0241", "Define the <PackageValidationBaselineVersion> node with a previously released stable version."));
}

public class Guards
{
    [TestCase("EmptyProject.cs")]
    [TestCase("PackageValidationDisabled.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefinePackageValidationBaselineVersion()
        .ForProject(project)
        .HasNoIssues();
}
