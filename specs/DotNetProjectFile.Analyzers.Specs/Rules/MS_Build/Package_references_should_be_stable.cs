namespace Rules.MS_Build.Package_references_should_be_stable;

public class Reports
{
    [Test]
    [Ignore("Buildalizer does not output any artifacts, hence nothing is analyzed.")]
    public void unstable_versions()
        => new PackageReferencesShouldBeStable()
        .ForProject("UnstablePackageReferences.cs")
        .HasIssues(
            new Issue("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'.").WithSpan(09, 04, 09, 85),
            new Issue("Proj1101", "Use a stable version of 'System.Text.Json', instead of *-*'." /*.................*/).WithSpan(10, 04, 10, 65));

    [Test]
    public void unstable_versions_via_CPM()
        => new PackageReferencesShouldBeStable()
        .ForProject("UnstableVersionsCPM.cs")
        .HasIssues(
            new Issue("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'.").WithSpan(08, 04, 08, 52),
            new Issue("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'.").WithSpan(09, 04, 09, 93));
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
