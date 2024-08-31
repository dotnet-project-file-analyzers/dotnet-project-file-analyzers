namespace Rules.MS_Build.Define_package_reference_version;

public class Reports
{
    [Test]
    public void on_no_output_type()
       => new DefinePackageReferenceVersion()
       .ForProject("PackageReferenceWithoutVersion.cs")
       .HasIssue(
           new Issue("Proj0804", "Define version for 'Warpstone' PackageReference."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefinePackageReferenceVersion()
        .ForProject(project)
        .HasNoIssues();
}
