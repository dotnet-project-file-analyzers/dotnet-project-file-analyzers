namespace Rules.MS_Build.Exclude_private_asset_dependencies;

public class Reports
{
    [Test]
    public void private_assets_not_being_private()
        => new ExcludePrivateAssetDependencies()
        .ForProject("PrivateAssets.cs")
        .HasIssues(
            new Issue("Proj2100", @"Mark the package reference ""coverlet.collector"" as a private asset.").WithSpan(16, 05, 16, 65),
            new Issue("Proj2100", @"Mark the package reference ""NUnit.Analyzers"" as a private asset.").WithSpan(17, 05, 17, 143));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new ExcludePrivateAssetDependencies()
        .ForProject(project)
        .HasNoIssues();
}
