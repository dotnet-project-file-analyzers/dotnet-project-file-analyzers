namespace Rules.MS_Build.Package_references_should_be_stable;

public class Reports
{
    [Test]
    public void unstable_versions()
        => new PackageReferencesShouldBeStable()
        .ForProject("UnstablePackageReferences.cs")
        .HasIssues(
            Issue.WRN("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'").WithSpan(08, 04, 08, 86).WithPath("UnstablePackageReferences.csproj"),
            Issue.WRN("Proj1101", "Use a stable version of 'System.Text.Json', instead of '*-*'" /*.................*/).WithSpan(09, 04, 09, 65).WithPath("UnstablePackageReferences.csproj"));

    [Test]
    public void unstable_versions_via_CPM()
        => new PackageReferencesShouldBeStable()
        .ForProject("UnstableVersionsCPM.cs")
        .HasIssues(
            Issue.WRN("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-rc.2.24473.5'").WithSpan(09, 04, 09, 88).WithPath("UnstableVersionsCPM.csproj"),
            Issue.WRN("Proj1101", "Use a stable version of 'Warpstone', instead of '2.0.0-preview2'").WithSpan(10, 04, 10, 67).WithPath("Directory.Packages.props"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project)
         => new AddAdditionalFile()
        .ForProject(project)
        .HasNoIssues();
}
