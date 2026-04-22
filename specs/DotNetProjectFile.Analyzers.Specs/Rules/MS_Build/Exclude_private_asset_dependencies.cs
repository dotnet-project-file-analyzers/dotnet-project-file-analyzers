namespace Rules.MS_Build.Exclude_private_asset_dependencies;

public class Reports
{
    [Test]
    public void private_assets_not_being_private() => new ExcludePrivateAssetDependencies().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <Nullable>enable</Nullable>
          </PropertyGroup>

          <ItemGroup Label="Compliant">
            <PackageReference Include="Microsoft.CodeAnalysis.Analyzers" Version="3.3.4" PrivateAssets="All" />
            <PackageReference Include="Qowaiv" Version="6.4.4" />
            <PackageReference Include="Qowaiv.Analyzers.CSharp" Version="2.0.1">
              <PrivateAssets>all</PrivateAssets>
            </PackageReference>
          </ItemGroup>

          <ItemGroup Label="Non-compliant">
            <PackageReference Include="coverlet.collector" Version="6.0.2" />
            <PackageReference Include="NUnit.Analyzers" Version="4.3.0" IncludeAssets="runtime; build; native; contentfiles; analyzers; buildtransitive" />
          </ItemGroup>

        </Project>
        """)
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
