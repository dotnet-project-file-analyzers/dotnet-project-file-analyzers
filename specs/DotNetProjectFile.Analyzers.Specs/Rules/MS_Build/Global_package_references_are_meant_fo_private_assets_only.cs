namespace Rules.MS_Build.Global_package_references_are_meant_fo_private_assets_only;

public class Reports
{
    [Test]
    public void global_package_reference_with_runtime_dependency()
        => new GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
        .ForProject("GlobalPackageReferenceRuntimeDependency.cs")
        .HasIssues(
            Issue.WRN("Proj0809", @"The global package reference 'Qowaiv' is not supposed to be a private asset")
                .WithSpan(07, 04, 07, 63)
                .WithPath("Directory.Packages.props"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("UseCPM.cs")]
    public void Projects_without_issues(string project) => new GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
        .ForProject(project)
        .HasNoIssues();
}
