namespace Rules.MS_Build.Exclude_private_asset_dependencies;

public class Reports
{
    [Test]
    public void private_assets_not_being_private()=> new ExcludePrivateAssetDependencies()
        .ForProject("PrivateAssets.cs")
        .HasIssues(
            Issue.WRN("Proj1200", @"Mark the package reference ""coverlet.collector"" as a private asset" /*.*/).WithSpan(16, 04, 16, 069),
            Issue.WRN("Proj1200", @"Mark the package reference ""NUnit.Analyzers"" as a private asset" /*....*/).WithSpan(17, 04, 17, 147));
}

public class Guards
{
    [Test]
    public void previously_reported_NuGet_Protocol() => new ExcludePrivateAssetDependencies()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <PackageReference Include=""NuGet.Protocol"" Version=""6.13.1"" />
              </ItemGroup>

            </Project>")
        .HasNoIssues();

    [Test]
    public void previously_reported_MemoryPack() => new ExcludePrivateAssetDependencies()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <PackageReference Include=""MemoryPack"" Version=""1.21.4"" />
              </ItemGroup>

            </Project>")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project) => new ExcludePrivateAssetDependencies()
        .ForProject(project)
        .HasNoIssues();
}
