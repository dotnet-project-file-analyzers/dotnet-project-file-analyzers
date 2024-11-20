namespace Rules.MS_Build.Generate_SBOM;

public class Reports
{
    [TestCase("PackageWithoutSBOM.cs")]
    [TestCase("PackageWithSBOMDisabled.cs")]
    public void on_no_is_publishable(string project) => new GenerateSbom()
        .ForProject(project)
        .HasIssue(new Issue("Proj0243", "Enable SBOM generation with <GenerateSBOM> is 'true' define the <IsPackable> node with value 'false'."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new GenerateSbom()
        .ForProject(project)
        .HasNoIssues();
}
