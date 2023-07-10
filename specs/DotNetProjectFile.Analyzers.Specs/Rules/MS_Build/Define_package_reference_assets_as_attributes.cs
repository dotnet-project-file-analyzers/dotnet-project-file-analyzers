namespace Rules.MS_Build.Define_package_reference_assets_as_attributes;

public class Reports
{
    [Test]
    public void assets_as_elements()
        => new DefinePackageReferenceAssetsAsAttributes()
        .ForProject("PackageReferenceAssetsAsElements.cs")
        .HasIssues(
            new Issue("Proj0005", "Define package reference assets of 'BackwardsCompatibleFeatures' as attributes.").WithSpan(8, 5, 10, 23),
            new Issue("Proj0005", "Define package reference assets of 'SonarAnalyzer.CSharp' as attributes.").WithSpan(11, 5, 14, 23));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("PackageReferenceAssetsAsAttributes.cs")]
    public void assets_as_attributes_or_without(string project)
         => new DefinePackageReferenceAssetsAsAttributes()
        .ForProject(project)
        .HasNoIssues();
}
