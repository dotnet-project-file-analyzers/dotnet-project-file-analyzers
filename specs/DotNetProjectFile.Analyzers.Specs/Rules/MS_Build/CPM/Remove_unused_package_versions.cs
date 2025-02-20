namespace Rules.MS_Build.CPM.Remove_unused_package_versions;

public class Reports
{
    [Test]
    public void project_with_unused_package_versions() => new RemoveUnusedPackageVersions()
        .ForSDkProject("CPMWithUnusedPackageVersions")
        .HasIssues(
            Issue.WRN("Proj0810", "Remove unused <PackageVersion> 'Newtonsoft.Json'" /*.*/).WithSpan(09, 04, 09, 65),
            Issue.WRN("Proj0810", "Remove unused <PackageVersion> 'Serilog'" /*.........*/).WithSpan(11, 04, 11, 56),
            Issue.WRN("Proj0810", "Remove unused <PackageVersion> 'System.Memory'" /*...*/).WithSpan(12, 04, 12, 62));
}

public class Guards
{
    [Test]
    public void Projects_with_CPM_file() => new RemoveUnusedPackageVersions()
       .ForProject("UseCPM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPM(string project) => new RemoveUnusedPackageVersions()
        .ForProject(project)
        .HasNoIssues();
}
