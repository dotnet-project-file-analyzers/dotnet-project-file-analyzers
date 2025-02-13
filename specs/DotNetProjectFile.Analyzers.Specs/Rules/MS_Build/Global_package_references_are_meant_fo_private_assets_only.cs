namespace Rules.MS_Build.Global_package_references_are_meant_fo_private_assets_only;

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("UseCPM.cs")]
    public void Projects_without_issues(string project) => new GlobalPackageReferencesAreMeantForPrivateAssetsOnly()
        .ForProject(project)
        .HasNoIssues();
}
