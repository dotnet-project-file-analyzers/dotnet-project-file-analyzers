namespace Rules.MS_Build.Enable_package_validation;

public class Reports
{
    [Test]
    public void on_missing_node()
       => new EnablePackageValidation()
       .ForProject("EmptyProject.cs")
       .HasIssue(
           new Issue("Proj0240", "Define the <EnablePackageValidation> node with value 'true', define the <IsPackable> node with value 'false' or define the <DevelopmentDependency> node with value 'false'."));

    [Test]
    public void on_disabled()
       => new EnablePackageValidation()
       .ForProject("PackageValidationDisabled.cs")
       .HasIssue(
           new Issue("Proj0240", "Define the <EnablePackageValidation> node with value 'true', define the <IsPackable> node with value 'false' or define the <DevelopmentDependency> node with value 'false'."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new EnablePackageValidation()
        .ForProject(project)
        .HasNoIssues();
}
