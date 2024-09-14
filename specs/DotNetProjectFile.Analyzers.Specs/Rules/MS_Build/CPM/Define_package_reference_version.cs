namespace Rules.MS_Build.Define_package_reference_version;

public class Reports
{
    /// <remarks>Should report but the build crashes.</remarks>
    [Test]
    public void missing_versions()
       => new DefinePackageReferenceVersion()
       .ForProject("PackageReferenceWithoutVersion.cs")
       .HasNoIssues();
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
