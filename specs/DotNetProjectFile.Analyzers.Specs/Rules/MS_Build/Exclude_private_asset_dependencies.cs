namespace Rules.MS_Build.Exclude_private_asset_dependencies;

public class Reports
{
    [Test]
    public void private_assets_not_being_private() => new ExcludePrivateAssetDependencies()
        .ForProject("PrivateAssets.cs")
        .HasIssues(
            Issue.WRN("Proj1200", @"Mark the package reference ""coverlet.collector"" as a private asset" /*.*/).WithSpan(16, 04, 16, 069),
            Issue.WRN("Proj1200", @"Mark the package reference ""NUnit.Analyzers"" as a private asset" /*....*/).WithSpan(17, 04, 17, 147));
}

public class Guards
{
    [Test]
    public void native_asssets() => new ExcludePrivateAssetDependencies().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="SkiaSharp.NativeAssets.Linux" Version="3.116.1" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void not_grouped_dependencies() => new ExcludePrivateAssetDependencies().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Qowaiv" Version="7.4.7" />
            <PackageReference Include="xunit" Version="2.9.3" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void previously_reported_NuGet_Protocol() => new ExcludePrivateAssetDependencies().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="NuGet.Protocol" Version="6.13.1" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();

    [Test]
    public void previously_reported_MemoryPack() => new ExcludePrivateAssetDependencies()
        .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
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
