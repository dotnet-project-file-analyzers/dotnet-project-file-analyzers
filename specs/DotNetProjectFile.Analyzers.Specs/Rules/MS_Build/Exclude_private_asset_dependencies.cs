namespace Rules.MS_Build.Exclude_private_asset_dependencies;

public class Reports
{
    [Test]
    public void private_assets_not_being_private()
        => new ExcludePrivateAssetDependencies()
        .ForProject("PrivateAssets.cs")
        .HasIssues(
            new Issue("Proj1200", @"Mark the package reference ""coverlet.collector"" as a private asset." /*.*/).WithSpan(16, 04, 16, 069),
            new Issue("Proj1200", @"Mark the package reference ""NUnit.Analyzers"" as a private asset." /*....*/).WithSpan(17, 04, 17, 147));
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
